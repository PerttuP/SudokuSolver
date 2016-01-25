using System;
using System.Collections.Generic;
using System.Linq;
using SudokuLib;

namespace SolverLib
{
    /// <summary>
    /// A simple implementation for ISudokuSolver.
    /// Uses only simple methods and trial-and-error to find solution. 
    /// </summary>
    /// 
    /// <remarks>
    /// This solver uses only simple methods and trial-and-error to find 
    /// solution. Not most sofisticated solution.
    /// </remarks>
    internal class SimpleSolver : ISudokuSolver
    {
        private SolverError _error;
        private int _guessLimit;
        

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="guessLimit">
        /// Maximum number of candidates in square that can be used in guessing.
        /// </param>
        /// <remarks>guessLimit must be equal or greater than 2.</remarks>
        public SimpleSolver(int guessLimit = 2)
        {
            System.Diagnostics.Debug.Assert(guessLimit >= 2);
            _error = null;
            _guessLimit = guessLimit;
        }


        /// <summary>
        /// Return Error message describing latest solving error.
        /// </summary>
        /// <returns>Error message.</returns>
        public SolverError LastError()
        {
            return _error;
        }


        /// <summary>
        /// Implements the ISudokuSolver interface. 
        /// Solves the given sudoku table.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>True, if table was solved successfully.</returns>
        public bool Solve(ref ITable table)
        {
            if (_error == null)
            {
                _error = new SolverError();
                _error.LastBeforeConflict = table;
            }

            try
            {
                while (!table.IsReady())
                {
                    if (!FindTrivial(table))
                    {
                        return TrialAndError(ref table);
                    }
                }
            }
            catch (ConflictException)
            {
                _error.ErrorString = "Solving ended in conflict.";
                return false;
            }

            _error = null;
            return true;
        }


        /// <summary>
        /// Implements the ISudokuSolver interface. Returns next applicaple 
        /// SolverAction. Does not modify the table itself.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>A valid SolverAction, if action was found.</returns>
        public SolverAction NextAction(ITable table)
        {
            if (table.IsReady())
            {
                _error = new SolverError("Already finished.");
                return new SolverAction();
            }
            _error = new SolverError();
            _error.LastBeforeConflict = table;
            _error.LastCertain = table;

            // Try finding a trivial action
            SolverAction a = this.FindTrivialAction(table);
            if (a.IsValid())
            {
                _error = null;
                return a;
            }
            return this.NextActionTrialAndError(table);
        }


        /// <summary>
        /// Find and set next number which is eather only candidate in 
        /// its square or number has no other possiple square in the group.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>
        /// True, if next number-location pair was found and set successfully.
        /// </returns>
        private bool FindTrivial(ITable table)
        {
            // Find number being only candidate in its square.
            SolverAction a = FindOnlyCandidate(table);
            if (a.IsValid())
            {
                _error.ConflictingAction = a;
                table.SetNumber(a.Location, a.Number);
                return true;
            }
            a = FindNoOtherInGroup(table);
            if (a.IsValid())
            {
                _error.ConflictingAction = a;
                table.SetNumber(a.Location, a.Number);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Find number that can be set to table, because a square has no other 
        /// candidates.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>Action required to set found number to the table.</returns>
        private SolverAction FindOnlyCandidate(ITable table)
        {
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    List<int> cands = table.CandidatesAt(c);
                    if (cands.Count == 1)
                    {
                        SolverAction a = new SolverAction(c, cands.ElementAt(0));
                        return a;
                    }
                }
            }
            return new SolverAction();
        }


        /// <summary>
        /// Find next number that has only one possiple square in a group.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>Action required to set next number to table.</returns>
        private SolverAction FindNoOtherInGroup(ITable table)
        {
            SolverAction a = new SolverAction();
            for (int i = 1; i < 10; ++i)
            {
                a = FindNoOtherInRowOrColumn(table, i);
                if (a.IsValid()) return a;
                a = FindNoOtherInRegion(table, i);
                if (a.IsValid()) return a;
            }
            return a;
        }


        /// <summary>
        /// Find next number that has only one possiple square on row/column.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <param name="index">row/column number.</param>
        /// <returns>Action required to set found number to table.</returns>
        private SolverAction FindNoOtherInRowOrColumn(ITable table, int index)
        {
            // Init structure for possiple locations for each number.
            Dictionary<int, List<Coordinate>> possibleOnRow = new Dictionary<int, List<Coordinate>>();
            Dictionary<int, List<Coordinate>> possibleOnCol = new Dictionary<int, List<Coordinate>>();
            for (int n = 1; n < 10; ++n)
            {
                possibleOnRow.Add(n, new List<Coordinate>());
                possibleOnCol.Add(n, new List<Coordinate>());
            }
            // Find possiple squares for each number.
            for (int i = 1; i < 10; ++i)
            {
                Coordinate cr = new Coordinate(index, i);
                Coordinate cc = new Coordinate(i, index);
                foreach (int n in table.CandidatesAt(cr))
                {
                    possibleOnRow[n].Add(cr);
                    possibleOnCol[n].Add(cc);
                }
            }
            // Check if any number have only 1 possiple location.
            for (int n = 1; n < 10; ++n)
            {
                if (possibleOnRow[n].Count == 1)
                {
                    return new SolverAction(possibleOnRow[n].ElementAt(0), n);
                }
                else if (possibleOnCol[n].Count == 1)
                {
                    return new SolverAction(possibleOnCol[n].ElementAt(0), n);
                }
            }
            return new SolverAction();
        }


        /// <summary>
        /// Find next number that has only one possiple square in region.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <param name="index">region id-number.</param>
        /// <returns>Action required to set found number to table.</returns>
        private SolverAction FindNoOtherInRegion(ITable table, int index)
        {
            // Init structure for storing possiple locations for each num.
            List<Coordinate> region = table.Regions().Region(index);
            Dictionary<int, List<Coordinate>> possiple = new Dictionary<int, List<Coordinate>>();
            for (int n = 1; n < 10; ++n)
            {
                possiple[n] = new List<Coordinate>();
            }
            // Find possiple locations for each num.
            foreach (Coordinate c in region)
            {
                foreach (int n in table.CandidatesAt(c))
                {
                    possiple[n].Add(c);
                }
            }
            // Check if any num have only 1 possiple location.
            foreach (int n in possiple.Keys)
            {
                if (possiple[n].Count == 1)
                {
                    return new SolverAction(possiple[n].ElementAt(0), n);
                }
            }
            return new SolverAction();
        }


        /// <summary>
        /// Try solving using Trial and error methodology.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>True, if table was solved successfully.</returns>
        private bool TrialAndError(ref ITable table)
        {
            // Find square with only 2 candidates.
            Coordinate loc = FindLeastCandidates(table);
            if (loc == null)
            {
                _error.ErrorString = "Could not find location to guess a number.";
                return false;
            }

            // Copy present table.
            ITable copy = (ITable)table.Clone();
            if (_error.LastCertain == null)
            {
                _error.LastCertain = copy;
            }

            // Guess each candidate at a time.
            for (int i=0; i<copy.CandidatesAt(loc).Count; ++i)
            {
                if (this.GuessNumber(ref table, loc, i))
                {
                    return true;
                }
                table = (ITable)copy.Clone();
                _error.LastBeforeConflict = table;
            }
            return false;
        }


        /// <summary>
        /// Find a location for guess (square having only 2 candidates).
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>
        /// Location having least candidates (max defined by constructor parameter)
        /// or null if not found.
        /// </returns>
        private Coordinate FindLeastCandidates(ITable table)
        {
            int minCandidates = _guessLimit;
            Coordinate loc = null;

            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (table.EmptyAt(c) && 
                        table.CandidatesAt(c).Count <= minCandidates)
                    {
                        minCandidates = table.CandidatesAt(c).Count;
                        loc = c;
                    }
                }
            }
            return loc;
        }


        /// <summary>
        /// Find next trivial action. Does not modify table.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>Next applicaple trivial action.</returns>
        /// <remarks>
        /// Returns an invalid SolverAction, if no trivial action was found.
        /// </remarks>
        private SolverAction FindTrivialAction(ITable table)
        {
            SolverAction a = this.FindOnlyCandidate(table);
            if (a.IsValid())
            {
                return a;
            }
            return FindNoOtherInGroup(table);
        }


        /// <summary>
        /// Find next action using trial and error method. 
        /// Table is not modified.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>Next applicaple action.</returns>
        /// <remarks>
        /// Returns an invalid SolverAction, if table could not be solved.
        /// </remarks>
        private SolverAction NextActionTrialAndError(ITable table)
        {
            Coordinate loc = this.FindLeastCandidates(table);
            if (loc == null)
                return new SolverAction();

            for (int i=0; i<table.CandidatesAt(loc).Count; ++i)
            {
                ITable copy = (ITable)table.Clone();
                if (this.GuessNumber(ref copy, loc, i))
                {
                    int n = table.CandidatesAt(loc).ElementAt(i);
                    return new SolverAction(loc, n);
                }
            }
            return new SolverAction();
        }


        /// <summary>
        /// Guess next candidate as a correct number and try to solve.
        /// </summary>
        /// <param name="table">Table to be solved. Will be modified.</param>
        /// <param name="loc">Location for guessing.</param>
        /// <param name="index">Candidate index.</param>
        /// <returns>True, if guess lead to solution.</returns>
        private bool GuessNumber(ref ITable table, Coordinate loc, int index)
        {
            try
            {
                table.SetNumber(loc, table.CandidatesAt(loc).ElementAt(index));
                return this.Solve(ref table);
            }
            catch (ConflictException)
            {
                return false;
            }
        }
    }
}
