using System.Collections.Generic;
using System;
using System.Diagnostics;

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
        /// <remarks>Num must be in range 1-9.</remarks>
        public Square(int num)
        {
            Debug.Assert(num > 0 && num < 10, "Number out of range!");
            number = num;
            candidates = new SortedSet<int>();
        }


        /// <summary>
        /// Construct new Square having given candidates.
        /// </summary>
        /// <param name="cands">Candidates to be assigned to the square.</param>
        /// <remarks>All candidates must be in range 1-9.</remarks>
        public Square(ICollection<int> cands)
        {
            number = 0;
            candidates = new SortedSet<int>();
            foreach (int n in cands)
            {
                Debug.Assert(n > 0 && n < 10, "Candidate out of range!");
                candidates.Add(n);
            }
        }


        /// <summary>
        /// The number assigned to the square. 
        /// </summary>
        /// <remarks>
        /// Only values 1-9 are allowed. Assigning a new number clears old value 
        /// and candidates. Value 0 returned from getter means that square is empty. 
        /// </remarks>
        public int Number
        {
            get { return number; }
            set
            {
                Debug.Assert(value > 0 && value < 10, "Square value out of range!");
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
        /// <remarks>Only values 1-9 are allowed as parameters.</remarks>
        public void InsertCandidate(int value)
        {
            Debug.Assert(this.Empty(), "Attempted to insert candidate to non-empty square.");
            Debug.Assert(value > 0 && value < 10, "Candidate out of range!");
            candidates.Add(value);
        }


        /// <summary>
        /// Add multiple candidates at once.
        /// </summary>
        /// <param name="values">New candidates.</param>
        /// <remarks>Only values 1-9 are allowed as parameters.</remarks>
        public void InsertCandidate(ICollection<int> values)
        {
            Debug.Assert(this.Empty(), "Attempted to insert candidate to non-empty square.");
            foreach (int n in values)
            {
                Debug.Assert(n > 0 && n < 10, "Candidate out of range!");
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
