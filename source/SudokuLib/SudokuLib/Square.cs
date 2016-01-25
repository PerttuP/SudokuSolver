using System.Collections.Generic;
using System;


/// <summary>
/// This namespace contains basic utilities to create a sudoku game.
/// </summary>
namespace SudokuLib
{
    /// <summary>
    /// The Square class represents an individual square in sudoku table. 
    /// Square object holds the number or candidates assigned to it.
    /// </summary>
    internal class Square : ICloneable
    { 
        private int number;
        private SortedSet<int> candidates;


        /// <summary>
        /// Constructs new empty Square without any candidates.
        /// </summary>
        public Square()
        {
            number = 0;
            candidates = new SortedSet<int>();
        }


        /// <summary>
        /// Constructs new Square having number num assigned.
        /// </summary>
        /// <param name="num">Value to be assigned to the square.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Is thrown if num is out of range [1,9].
        /// </exception>
        public Square(int num)
        {
            if (num < 1 || num > 9)
            {
                throw new ArgumentOutOfRangeException("num", "Number out of range [1,9]!");
            }

            number = num;
            candidates = new SortedSet<int>();
        }


        /// <summary>
        /// Construct new Square having given candidates.
        /// </summary>
        /// <param name="cands">Candidates to be assigned to the square.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// is thrown if any of given candidates is out of range [1,9].
        /// </exception>
        public Square(ICollection<int> cands)
        {
            number = 0;
            candidates = new SortedSet<int>();
            foreach (int n in cands)
            {
                if (n < 1 || n > 9)
                {
                    throw new ArgumentOutOfRangeException("cands", "Candidate out of range [1,9]!");
                }
                candidates.Add(n);
            }
        }


        /// <summary>
        /// The number assigned to the square. 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Is thrown if value in setter is out of range [1,9].
        /// </exception>
        public int Number
        {
            get { return number; }
            set
            {
                if (value < 1 || value > 9)
                {
                    throw new ArgumentOutOfRangeException("value", "Square number out of range [1,9]!");
                }
                candidates.Clear();
                number = value;
            }
        }


        /// <summary>
        /// Get currently assigned values.
        /// </summary>
        /// <returns>Currently assigned candidates as a list.</returns>
        public List<int> Candidates()
        {
            List<int> rv = new List<int>();
            foreach (int n in candidates)
            {
                rv.Add(n);
            }
            return rv;
        }


        /// <summary>
        /// Check if square is empty (has no value assigned).
        /// </summary>
        /// <returns>True, if square is empty.</returns>
        public bool Empty()
        {
            return number == 0;
        }


        /// <summary>
        /// Clear assigned value and candidates.
        /// </summary>
        public void Clear()
        {
            number = 0;
            candidates.Clear();
        }

        
        /// <summary>
        /// Check if square has the given candidate assigned.
        /// </summary>
        /// <param name="value">Candidate value to be chacked.</param>
        /// <returns>True, if given value is one of assigned candidates.</returns>
        public bool HasCandidate(int value)
        {
            return candidates.Contains(value);
        }


        /// <summary>
        /// Remove given candidate from square's assigned candidates.
        /// If there is no such candidate, does nothing.
        /// </summary>
        /// <param name="value">Candidate to be removed.</param>
        public void RemoveCandidate(int value)
        {
            candidates.Remove(value);
        }


        /// <summary>
        /// Add new candidate into square.
        /// </summary>
        /// <param name="value">New candidate.</param>
        /// <exception cref="InvalidOperationException">
        /// Is thrown if square is not empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// is thrown if value is out of range [1,9].
        /// </exception>
        public void InsertCandidate(int value)
        {
            if (!this.Empty())
            {
                throw new InvalidOperationException("Cannot add candidates to a non-empty square.");
            }
            else if (value < 1 || value > 9)
            {
                throw new ArgumentOutOfRangeException("value", "Candidate out of range [1,9]!");
            }

            candidates.Add(value);
        }


        /// <summary>
        /// Add multiple candidates at once.
        /// </summary>
        /// <param name="values">New candidates.</param>
        /// <exception cref="InvalidOperationException">
        /// Is thrown if square is not empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Is thrown if any of given candidates is out of range [1,9].
        /// </exception>
        public void InsertCandidate(ICollection<int> values)
        {
            if (!this.Empty())
            {
                throw new InvalidOperationException("Cannot add candidates to a non-empty square.");
            }
            foreach (int n in values)
            {
                if (n < 1 || n > 9)
                {
                    throw new ArgumentOutOfRangeException("value", "Candidate out of range [1,9]!");
                }

                candidates.Add(n);
            }
        }


        /// <summary>
        /// Implements the ICloneable interface.
        /// </summary>
        /// <returns>Deep copy of this object</returns>
        public object Clone()
        {
            if (!this.Empty())
            {
                return new Square(this.Number);
            }
            else
            {
                return new Square(this.Candidates());
            }
        }
    }
}
