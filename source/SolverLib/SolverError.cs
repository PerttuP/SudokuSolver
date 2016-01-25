using System;
using SudokuLib;

namespace SolverLib
{
    /// <summary>
    /// SolverError object contains data describing errors 
    /// occured during failed attempt to solve a sudoku table.
    /// </summary>
    public class SolverError
    {
        private string _errorStr;
        private ITable _lastCertain;
        private ITable _lastBeforeConflict;
        private SolverAction _conflictAction;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg">Error message.</param>
        /// <param name="lastCertain">
        /// Last certainly valid table situation.
        /// </param>
        /// <param name="lastBeforeConflict">
        /// "Last table situation before detecting conflict.
        /// </param>
        /// <remarks>
        /// LastCertain, lastBeforeConflict and conflictingAction 
        /// may be null, if they are irrelevant to the error.
        /// </remarks>
        public SolverError(
            string msg = "", 
            ITable lastCertain = null, 
            ITable lastBeforeConflict = null,
            SolverAction conflictingAction = null)
        {
            _errorStr = msg;
            _conflictAction = conflictingAction;
            if (lastCertain != null)
                _lastCertain = (ITable)lastCertain.Clone();
            if (lastBeforeConflict != null)
                _lastBeforeConflict = (ITable)lastBeforeConflict.Clone();
        }


        /// <summary>
        /// Error string describing the error.
        /// </summary>
        public String ErrorString
        {
            get { return _errorStr; }
            set { _errorStr = value; }
        }


        /// <summary>
        /// Last situation known to be valid (no guesses made).
        /// </summary>
        /// <remarks>May be null, if irrelevant to the error.</remarks>
        public ITable LastCertain
        {
            get { return _lastCertain; }
            set { _lastCertain = value; }
        }


        /// <summary>
        /// Last situation before conflicting action.
        /// </summary>
        /// /// <remarks>May be null, if irrelevant to the error.</remarks>
        public ITable LastBeforeConflict
        {
            get { return _lastBeforeConflict; }
            set { _lastBeforeConflict = value; }
        }


        /// <summary>
        /// Action causing conflict from LastBeforeError.
        /// </summary>
        /// /// <remarks>May be null, if irrelevant to the error.</remarks>
        public SolverAction ConflictingAction
        {
            get { return _conflictAction; }
            set { _conflictAction = value; }
        }
    }
}
