using System;
using System.Diagnostics;

namespace SudokuLib
{
    /// <summary>
    /// Represents a location on sudoku table. 
    /// Row and Column are values in range 1-9. 
    /// (1,1) is in left top corner.
    /// </summary>
    public class Coordinate : ICloneable
    {
        private readonly int r;
        private readonly int c;

        /// <summary>
        /// The row number.
        /// </summary>
        public int Row
        {
            get { return r; }
        }

        /// <summary>
        /// The column number.
        /// </summary>
        public int Column
        {
            get { return c; }
        }


        /// <summary>
        /// Constructs new Coordinate with given row and column values.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <param name="column">The column number.</param>
        /// <remarks>Only values 1-9 are allowed as row and column.</remarks>
        public Coordinate(int row, int column)
        {
            Debug.Assert(row > 0 && row < 10);
            Debug.Assert(column > 0 && column < 10);
            r = row;
            c = column;
        }


        /// <summary>
        /// Implements the ICloneable interface.
        /// </summary>
        /// <returns>Deep copy of this object.</returns>
        public object Clone()
        {
            return new Coordinate(r, c);
        }


        /// <summary>
        /// Compare two coordinates for equality.
        /// </summary>
        /// <param name="other">Another coordinate.</param>
        /// <returns>
        /// True, if 'other' is a coordinate and has same row and 
        /// column as this.
        /// </returns>
        public override bool Equals(object other)
        {
            Coordinate c = (Coordinate)other;
            if (other == null)
            {
                return false;
            }
            else
            {
                return c.Row == this.r && c.Column == this.c;
            }
        }


        /// <summary>
        /// Overrides object.GetHashCode().
        /// </summary>
        /// <returns>Hash code for this object.</returns>
        public override int GetHashCode()
        {
            // Each valid coordinate has unique hash code in range [0,99].
            return (r - 1) * 10 + (c - 1);
        }


        public override string ToString()
        {
            return "(" + Row.ToString() +  "," + Column.ToString() +  ")";
        }

        public static Coordinate Parse(string s)
        {
            if (s == null || s.Length != 5 || s[0] != '(' || s[4] != ')' || s[2] != ',')
            {
                return null;
            }
            int row = (int)Char.GetNumericValue(s[1]);
            int col = (int)Char.GetNumericValue(s[3]);
            if (row == 0 || col == 0) return null;
            return new Coordinate(row, col);
        }
    }
}
