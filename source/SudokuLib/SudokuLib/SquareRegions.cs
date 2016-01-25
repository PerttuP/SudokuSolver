using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace SudokuLib
{
    /// <summary>
    /// Defines the square groupings ("Regions").
    /// </summary>
    public class SquareRegions : ICloneable
    {
        private readonly List<List<Coordinate>> regions;

        /// <summary>
        /// Create and return the default SquareRegions.
        /// </summary>
        /// <returns>The default SquareRegions.</returns>
        /// <remarks>
        /// The default square regions are defined as follows:
        /// Regions consist of a 3x3 square area. Region-id:s are as below.
        /// 
        ///     1  2  3
        ///     4  5  6
        ///     7  8  9
        ///    
        /// Order of coordinates inside regions is unspecified.
        /// </remarks>
        public static SquareRegions DefaultRegions()
        {
            List<List<Coordinate>> regions = new List<List<Coordinate>>(9);
            for (int i=0; i<9; ++i)
            {
                regions.Add( new List<Coordinate>() );
            }

            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    int rowFactor = (int)Math.Ceiling(row / 3.0) - 1;
                    int colFactor = (int)Math.Ceiling(col / 3.0);
                    int regionId = rowFactor * 3 + colFactor;
                    regions[regionId - 1].Add(c);
                }
            }

            SquareRegions rv = new SquareRegions();
            for (int i=0; i<9; ++i)
            {
                rv.SetRegion(regions[i], i + 1);
            }
            return rv;
        } 


        /// <summary>
        /// Constructs an empty, invalid SquareRegions.
        /// </summary>
        public SquareRegions()
        {
            regions = new List<List<Coordinate>>();
            for (int i=0; i<9; ++i)
            {
                regions.Add(null);
            }
        }


        /// <summary>
        /// Check if regions are valid.
        /// </summary>
        /// <returns>True, if SquareRegions is valid.</returns>
        /// <remarks>
        /// In valid region, each region has 9 squares, and each square is in 
        /// exactly one region. All squares in region must be connected to 
        /// each others. There are 9 regions.
        /// </remarks>
        public bool IsValid()
        {
            List<Coordinate> allCoordinates = new List<Coordinate>();
            foreach (List<Coordinate> region in regions)
            {
                // Region is defined and has correct number of squares.
                if (region == null || region.Count() != 9)
                {
                    return false;
                }
                else if (!AreConnected(region))
                {
                    return false;
                }

                // Check for dublicate coordinates, and that all squares in
                // region are not on same row/column.
                bool sameRow = true;
                bool sameCol = true;
                int firstRow = region.ElementAt(0).Row;
                int firstCol = region.ElementAt(0).Column;
                foreach (Coordinate c in region)
                {
                    if (c.Row != firstRow)
                    {
                        sameRow = false;
                    }
                    if (c.Column != firstCol)
                    {
                        sameCol = false;
                    }
                    if (allCoordinates.Contains(c))
                    {
                        return false;
                    }
                    else allCoordinates.Add(c);
                } 
                if (sameRow || sameCol)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Insert a new region.
        /// </summary>
        /// <param name="coordinates">List of square locations associated to the region.</param>
        /// <param name="id">Region's unique id.</param>
        /// <remarks>
        /// Id must be in range 1-9. None of coordinates should be null.
        /// </remarks>
        public void SetRegion(IEnumerable<Coordinate> coordinates, int id)
        {
            Debug.Assert(id > 0 && id < 10);
            foreach (Coordinate c in coordinates)
            {
                Debug.Assert(c != null);
            }
            regions[id - 1] = new List<Coordinate>(coordinates);
        }


        /// <summary>
        /// Get coorditates of region with given id.
        /// </summary>
        /// <param name="id">Region's unique id.</param>
        /// <returns>
        /// List of coordinates assigned to the region, 
        /// or null if the region is undefined (invalid regions).
        /// </returns>
        /// <remarks>
        /// The caller should not trust in returned coordinates if IsValid() retirns false.
        /// </remarks>
        /// 
        public List<Coordinate> Region(int id)
        {
            Debug.Assert(id > 0 && id < 10);
            return regions.ElementAt(id - 1);
        }


        /// <summary>
        /// Check if collection of coordinates are connected to each other.
        /// </summary>
        /// <param name="coordinates">Collection of coordinates.</param>
        /// <returns>True, if Coordinates form a continious area.</returns>
        /// <remarks>
        /// Note: Does not check if parameter contains equal coordinates.
        /// </remarks>
        private bool AreConnected(IEnumerable<Coordinate> coordinates)
        {
            List<Coordinate> connected = new List<Coordinate>();
            List<Coordinate> notConnected = new List<Coordinate>(coordinates);
            connected.Add(notConnected.ElementAt(0));
            notConnected.RemoveAt(0);

            int i = 0;
            while (i < notConnected.Count())
            {
                bool connectionFound = false;
                Coordinate ca = notConnected.ElementAt(i);
                // Check if a not-connected is next to any of connected ones.
                foreach (Coordinate cb in connected)
                {
                    if (IsConnected(ca, cb))
                    {
                        connectionFound = true;
                        break;
                    }
                }
                // Move to connected or check next non-connected.
                if (connectionFound)
                {
                    connected.Add(ca);
                    notConnected.RemoveAt(i);
                    i = 0;
                }
                else
                {
                    ++i;
                }
            }
            return notConnected.Count() == 0;
        }


        /// <summary>
        /// Check if two coordinates point to squares next to each other.
        /// </summary>
        /// <param name="ca">Coordinate a.</param>
        /// <param name="cb">Coordinate b.</param>
        /// <returns>True, if squares are next to each other.</returns>
        private bool IsConnected(Coordinate ca, Coordinate cb)
        {
            if (ca.Row == cb.Row)
            {
                return ca.Column == cb.Column - 1 || ca.Column == cb.Column + 1;
            }
            else if (ca.Column == cb.Column)
            {
                return ca.Row == cb.Row - 1 || ca.Row == cb.Row + 1;
            }
            else return false;
        }


        /// <summary>
        /// Implements the ICloneable interface.
        /// </summary>
        /// <returns>Deep copy of this object</returns>
        public object Clone()
        {
            SquareRegions clone = new SquareRegions();
            for (int i=1; i<10; ++i)
            {
                List<Coordinate> region = this.Region(i);
                List<Coordinate> regionClone = new List<Coordinate>();
                foreach (Coordinate c in region)
                {
                    regionClone.Add((Coordinate)c.Clone());
                }
                clone.SetRegion(regionClone, i);
            }
            return clone;
        }


        /// <summary>
        /// Check if two SquareRegions have equivalent regions. 
        /// Region id's or order of coordinates in them do not matter.
        /// Invalid regions are always considered not equivalent.
        /// </summary>
        /// <param name="other">Regions to be compared.</param>
        /// <returns>True, if regions are equivalent.</returns>
        public bool IsEquivalent(SquareRegions other)
        {
            // Return false on invalid regions. true if same objects.
            if (other == null) return false;
            else if (!this.IsValid() || !other.IsValid()) return false;
            else if (this == other) return true;

            List<int> matchedIDs = new List<int>();
            for (int i=1; i<10; ++i)
            {
                List<Coordinate> regionA = this.Region(i);
                bool matchFound = true;
                for (int j=1; j<10; ++j)
                {
                    // Skip already matched regions.
                    if (matchedIDs.Contains(j)) continue;

                    matchFound = true;
                    List<Coordinate> regionB = other.Region(j);
                    foreach (Coordinate c in regionA)
                    {
                        if (!regionB.Contains(c))
                        {
                            matchFound = false;
                            break;
                        }
                    }
                    if (matchFound)
                    {
                        matchedIDs.Add(j);
                        break;
                    }
                }

                if (!matchFound) return false;
            }
            return true;
        }


        /// <summary>
        /// Get string representation.
        /// </summary>
        /// <returns>String representation in format {reg1/rer2/.../reg9}, 
        /// where each region (reg is of format {c1,c2,...cn}, c being a 
        /// string representation of a coordinate.</returns>
        public override string ToString()
        {
            List<String> regions = new List<string>();
            for (int i=1; i<10; ++i)
            {
                List<string> regStr = new List<string>();
                foreach (Coordinate c in Region(i))
                {
                    regStr.Add(c.ToString());
                }
                regions.Add(String.Join(";", regStr));
            }
            return String.Join("/", regions);
        }


        /// <summary>
        /// Parse SquareRegions object from string.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <returns>
        /// New SquareRegions object or null, if parsing fails. 
        /// SquareRegions object may be invalid, if data is.
        /// </returns>
        public static SquareRegions Parse(string s)
        {
            SquareRegions rv = new SquareRegions();
            List<String> regions = new List<String>(s.Split('/'));
            for (int i=0; i<regions.Count; ++i)
            {
                if (regions[i].Length == 0)
                {
                    continue;
                }
                List<String> locations = new List<string>(regions[i].Split(';'));
                List<Coordinate> coords = new List<Coordinate>();
                foreach (string loc in locations)
                {
                    Coordinate c = Coordinate.Parse(loc);
                    if (c == null)
                    {
                        return null;
                    }
                    coords.Add(c);
                }
                rv.SetRegion(coords, i + 1);
            }
            return rv;
        }
    }
}
