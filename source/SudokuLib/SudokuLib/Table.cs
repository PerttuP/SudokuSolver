using System.Collections.Generic;
using System.Linq;
using System;

namespace SudokuLib
{
    /// <summary>
    /// Represents the sudoku table. Table keeps track on assigned 
    /// numbers and automatically handles candidates.
    /// </summary>
    internal class Table : ITable
    {
        private class Associations
        {
            public Group row, column, region;
            public int regionID;
        }

        private readonly SquareRegions _layout;
        private readonly Dictionary<Coordinate, Square> _squares;
        private readonly Dictionary<Coordinate, Associations> _associations;
        private readonly Dictionary<Coordinate, int> _inital_values;


        /// <summary>
        /// Construct empty Table using default grouping.
        /// </summary>
        public Table()
        {
            _layout = SquareRegions.DefaultRegions();
            _squares = new Dictionary<Coordinate, Square>();
            _associations = new Dictionary<Coordinate, Associations>();
            _inital_values = new Dictionary<Coordinate, int>();
            this.SetUpEmpty();
        }


        /// <summary>
        /// Construct new Table using given initial values.
        /// </summary>
        /// <param name="init_values">Initial values as coordinate-number pairs.</param>
        /// <param name="regions">Sqúare region grid.</param>
        /// <exception cref="ConflictException">
        /// Thrown when initial values contradict each others.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when given layout is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when any of initial numbers is out of range [1,9].
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when init_values is null.
        /// </exception>
        /// <remarks>
        /// If regions is null, default regions are used.
        /// </remarks>
        public Table(
            IDictionary<Coordinate, int> init_values,
            SquareRegions regions = null
            )
        {
            if (init_values == null)
            {
                throw new ArgumentNullException("init_values", "Initial values should not be null.");
            }
            else if (regions == null)
            {
                _layout = SquareRegions.DefaultRegions();
            }
            else
            {
                if (!regions.IsValid())
                {
                    throw new ArgumentException("layout", "Layout must be valid.");
                }
                _layout = regions;
            }
            _squares = new Dictionary<Coordinate, Square>();
            _associations = new Dictionary<Coordinate, Associations>();
            _inital_values = new Dictionary<Coordinate, int>(init_values);
            this.SetUpEmpty();

            // Insert initial values. May throw ConflictException.
            foreach (KeyValuePair<Coordinate, int> pair in _inital_values)
            {
                this.SetNumber(pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// Get number assigned to square in location pointed by given coordinate. 
        /// </summary>
        /// <param name="location">Square location.</param>
        /// <returns>Square's number, or 0 if square is empty.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when location is null.
        /// </exception>
        public int NumberAt(Coordinate location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location", "Location should not be null.");
            }
            return _squares[location].Number;
        }


        /// <summary>
        /// Check if square at given location is empty.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <returns>True, if square is empty.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown, when location is null.
        /// </exception>
        public bool EmptyAt(Coordinate location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location", "Location should not be null.");
            }
            return _squares[location].Empty();
        }


        /// <summary>
        /// Get candidates of square at given location.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <returns>List of candidates.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown, when location is null.
        /// </exception>
        public List<int> CandidatesAt(Coordinate location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location", "Location should not be null.");
            }
            return _squares[location].Candidates();
        }


        /// <summary>
        /// Check if Table is ready (all squares are filled correctly).
        /// </summary>
        /// <returns>True, if table is ready.</returns>
        public bool IsReady()
        {
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (_squares[c].Empty())
                    {
                        return false;
                    }
                }
            }

            foreach (Associations a in _associations.Values)
            {
                if (a.row.CheckConflict() ||
                    a.column.CheckConflict() ||
                    a.region.CheckConflict())
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Assign number to square at given location.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <param name="number">Number to be assigned.</param>
        /// <exception cref="ConflictException">
        /// Thrown, if assignment causes a conflict (basic guarantee).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown, if location is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown, when number is out of range [0,9].
        /// </exception>
        /// <remarks>
        /// Number must be in range 0-9. 
        /// If number is 0, the square's current number is removed.
        /// </remarks>
        public void SetNumber(Coordinate location, int number)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location", "Location should not be null.");
            }

            if (number == 0)
            {
                this.ClearSquare(location);
                return;
            }
            else if (!_squares[location].Empty())
            {
                this.ClearSquare(location);
            }

            _squares[location].Number = number;
            Associations a = _associations[location];
            a.row.ReduceCandidates();
            a.column.ReduceCandidates();
            a.region.ReduceCandidates();

            if (this.CheckConflict())
            {
                throw new ConflictException();
            }
        }


        /// <summary>
        /// Revert Table to its initial state.
        /// </summary>
        public void Reset()
        {
            _squares.Clear();
            _associations.Clear();
            this.SetUpEmpty();
            foreach (KeyValuePair<Coordinate, int> pair in _inital_values)
            {
                this.SetNumber(pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// Get table's square regions.
        /// </summary>
        /// <returns>SquareRegions representing the grouping.</returns>
        public SquareRegions Regions()
        {
            return (SquareRegions)_layout.Clone();
        }


        /// <summary>
        /// Get list of coordinates of squares that belong in same region 
        /// with given square.
        /// </summary>
        /// <param name="location">Reference square.</param>
        /// <returns>List of coordinates pointing to squares in same region.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown, if location is null.
        /// </exception>
        public List<Coordinate> GetRegionOf(Coordinate location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location", "Location should not be null.");
            }

            List<Coordinate> rv = new List<Coordinate>();
            int id = _associations[location].regionID;
            // Copy coordinates to return value.
            foreach (Coordinate c in _layout.Region(id))
            {
                rv.Add((Coordinate)c.Clone());
            }
            return rv;
        }


        /// <summary>
        /// Implements the ICloneable interface.
        /// </summary>
        /// <returns>Deep copy of this object.</returns>
        public object Clone()
        {
            Table copy = new Table(this._inital_values, this._layout);

            // Minimize overhead by inserting square attributes directly
            // without conflict checks.
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (_inital_values.Keys.Contains(c)) continue;

                    Square s1 = this._squares[c];
                    Square s2 = copy._squares[c];
                    if (s1.Empty())
                    {
                        s2.Clear();
                        s2.InsertCandidate(s1.Candidates());
                    }
                    else
                    {
                        s2.Number = s1.Number;
                    }
                }
            }

            return copy;
        }


        /// <summary>
        /// Set up a valid, empty table (helper for constructors).
        /// </summary>
        private void SetUpEmpty()
        {
            this.PopulateStructures();

            // Gather squares for each row and column.
            List<List<Square>> rows = new List<List<Square>>();
            List<List<Square>> columns = new List<List<Square>>();
            List<List<Square>> regions = new List<List<Square>>();
            for (int i = 0; i < 9; ++i)
            {
                rows.Add(new List<Square>());
                columns.Add(new List<Square>());
                regions.Add(new List<Square>());
            }

            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    rows[row - 1].Add(_squares[c]);
                    columns[col - 1].Add(_squares[c]);
                }
            }

            // Gather squares for each region.
            for (int i = 1; i < 10; ++i)
            {
                List<Coordinate> region = _layout.Region(i);
                foreach (Coordinate c in region)
                {
                    regions[i - 1].Add(_squares[c]);
                }
            }

            this.SetAssociations(rows, columns, regions);
        }


        /// <summary>
        /// Creates correct number of squares and associations. 
        /// These are modified later.
        /// </summary>
        private void PopulateStructures()
        {
            // Create squares and populate _squares and _associations.
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    Square s = new Square(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                    _squares.Add(c, s);
                    _associations.Add(c, new Associations());
                }
            }
        }


        /// <summary>
        /// Creates Groups and associates them with square coordinates.
        /// </summary>
        /// <param name="rows">Squares on same rows.</param>
        /// <param name="columns">Squares on same column.</param>
        /// <param name="regions">Squares on same region.</param>
        private void SetAssociations(
            List<List<Square>> rows,
            List<List<Square>> columns,
            List<List<Square>> regions
            )
        {
            // Set associations.
            for (int i = 0; i < 9; ++i)
            {
                Group rowGroup = new Group(rows[i]);
                Group columnGroup = new Group(columns[i]);
                Group regionGroup = new Group(regions[i]);

                for (int j = 1; j < 10; ++j)
                {
                    _associations[new Coordinate(i + 1, j)].row = rowGroup;
                    _associations[new Coordinate(j, i + 1)].column = columnGroup;
                }
                foreach (Coordinate c in _layout.Region(i + 1))
                {
                    _associations[c].region = regionGroup;
                    _associations[c].regionID = i + 1;
                }
            }
        }


        /// <summary>
        /// Remove square's current value and re-calculate all candidates.
        /// </summary>
        /// <param name="location">Square's location.</param>
        private void ClearSquare(Coordinate location)
        {
            Square s = _squares[location];
            int oldVal = s.Number;
            s.Clear();
            s.InsertCandidate(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            if (oldVal != 0)
            {
                // Find possiple squares for removed value.
                for (int i = 1; i < 10; ++i)
                {
                    Square onRow = _squares[new Coordinate(location.Row, i)];
                    Square onCol = _squares[new Coordinate(i, location.Column)];
                    if (onRow.Empty())
                    {
                        onRow.InsertCandidate(oldVal);
                    }
                    if (onCol.Empty())
                    {
                        onCol.InsertCandidate(oldVal);
                    }

                    int id = _associations[location].regionID;
                    List<Coordinate> region = _layout.Region(id);
                    foreach (Coordinate c in region)
                    {
                        if (_squares[c].Empty())
                            _squares[c].InsertCandidate(oldVal);
                    }
                }
            }
            this.ReduceCandidates();
        }


        /// <summary>
        /// Reduce candidates in all groups.
        /// </summary>
        private void ReduceCandidates()
        {
            SortedSet<int> reducedRows = new SortedSet<int>();
            SortedSet<int> reducedCols = new SortedSet<int>();
            SortedSet<int> reducedRegs = new SortedSet<int>();

            foreach (KeyValuePair<Coordinate,Associations> pair in _associations)
            {
                if (!reducedRows.Contains(pair.Key.Row))
                {
                    pair.Value.row.ReduceCandidates();
                    reducedRows.Add(pair.Key.Row);
                }
                if (!reducedCols.Contains(pair.Key.Column))
                {
                    pair.Value.column.ReduceCandidates();
                    reducedCols.Add(pair.Key.Column);
                }
                if (!reducedRegs.Contains(pair.Value.regionID))
                {
                    pair.Value.region.ReduceCandidates();
                    reducedRegs.Add(pair.Value.regionID);
                }
                
            }
        }


        /// <summary>
        /// Check conflicts in every group.
        /// </summary>
        /// <returns>True, if conflict was detected.</returns>
        private bool CheckConflict()
        {
            SortedSet<int> checkedRows = new SortedSet<int>();
            SortedSet<int> checkedCols = new SortedSet<int>();
            SortedSet<int> checkedRegs = new SortedSet<int>();

            foreach (KeyValuePair<Coordinate,Associations> pair in _associations)
            {
                
                if (!checkedRows.Contains(pair.Key.Row))
                {
                    if (pair.Value.row.CheckConflict()) return true;
                    checkedRows.Add(pair.Key.Row);
                }
                if (!checkedCols.Contains(pair.Key.Column))
                {
                    if (pair.Value.column.CheckConflict()) return true;
                    checkedCols.Add(pair.Key.Column);
                }
                if (!checkedRegs.Contains(pair.Value.regionID))
                {
                    if (pair.Value.region.CheckConflict()) return true;
                    checkedRegs.Add(pair.Value.regionID);
                }
            }
            return false;
        }
    }
}
