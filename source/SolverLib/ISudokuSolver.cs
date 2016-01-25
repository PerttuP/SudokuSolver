
namespace SolverLib
{
    /// <summary>
    /// Abstract interface for sudoku solvers.
    /// </summary>
    public interface ISudokuSolver
    {
        /// <summary>
        /// Solves the given sudoku table. Modifies the parameter.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>True, if table was solved successfully.</returns>
        /// <remarks>
        /// Table is left in a valid but unspecified state, if solving fails.
        /// </remarks>
        bool Solve(ref SudokuLib.ITable table);


        /// <summary>
        /// Find next action that can be made on table.
        /// </summary>
        /// <param name="table">Table to be solved.</param>
        /// <returns>A valid SolverAction, if next action was found.</returns>
        /// <remarks>Unlike in Solve, table is not modified here.</remarks>
        SolverAction NextAction(SudokuLib.ITable table);


        /// <summary>
        /// Get latest error.
        /// </summary>
        /// <returns>Error describing latest error in solving. 
        /// Returns null if no error occured.</returns>
        SolverError LastError();
    }
}
