using System.Collections.Generic;
using System.Linq;
using System;

namespace SudokuLib
{
    /// <summary>
    /// Represents group of 9 sudoku squares (row, column or region).
    /// </summary>
    internal class Group
    {
        private readonly List<Square> members;

        /// <summary>
        /// Construct new group for given squares.
        /// </summary>
        /// <param name="squares">Group members.</param>
        /// <exception cref="ArgumentException">
        /// Is thrown, if number of squares is not 9 or there are duplicate squares.
        /// </exception>
        public Group(ICollection<Square> squares)
        {
            if (squares.Count() != 9)
            {
                throw new ArgumentException("Group must have exactly 9 members.", "squares");
            }

            for (int i=0; i<9; ++i)
            {
                for (int j=i+1; j<9; ++j)
                {
                    if ( squares.ElementAt(i).Equals( squares.ElementAt(j) ))
                    {
                        throw new ArgumentException("All squares in group must be different objects.", "squares");
                    }
                }
            }

            members = new List<Square>(squares);
        }


        /// <summary>
        /// Remove candidates that cannot fit into squares based on other 
        /// members of this group.
        /// </summary>
        public void ReduceCandidates()
        {
            // Remove assigned values from empty square's candidates.
            List<int> nums = this.GetAssignedNumbers();
            foreach (Square s in members)
            {
                if (s.Empty())
                {
                    foreach(int i in nums)
                    {
                        s.RemoveCandidate(i);
                    }
                }
            }
        }


        /// <summary>
        /// Check for conflicting squares. Check is based on information 
        /// from this group only.
        /// </summary>
        /// <returns>True, if conflict is detected.</returns>
        public bool CheckConflict()
        {
            SortedSet<int> unassigned = new SortedSet<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            // Find dublicates
            List<int> assigned = new List<int>();
            foreach( Square s in members)
            {
                if (!s.Empty())
                {
                    if (assigned.Contains(s.Number))
                    {
                        // Dublicate found.
                        return true;
                    }
                    else
                    {
                        assigned.Add(s.Number);
                    }
                }
                else if (s.Candidates().Count() == 0)
                {
                    // An empty square without any candidates.
                    return true;
                }
            }

            // Check that all unassigned numbers have at least one suitable square. 
            foreach (int i in assigned)
            {
                unassigned.Remove(i);
            }
            foreach ( Square s in members)
            {
                foreach (int i in s.Candidates())
                {
                    if (unassigned.Contains(i))
                    {
                        unassigned.Remove(i);
                    }
                }
            }
            // If no conflict, unassigned should be empty.
            return unassigned.Count != 0;
        }


        /// <summary>
        /// Gather numbers that are assigned to any of squares in this group.
        /// </summary>
        /// <returns>List of assigned numbers.</returns>
        private List<int> GetAssignedNumbers()
        {
            List<int> nums = new List<int>();
            foreach (Square s in members)
            {
                if (!s.Empty())
                {
                    nums.Add(s.Number);
                }
            }
            return nums;
        }
    }
}
