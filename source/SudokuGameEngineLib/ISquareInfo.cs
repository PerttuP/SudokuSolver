using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Enumeration type for square number source.
    /// </summary>
    public enum SquareNumberSource
    {
        /// <summary>
        /// Number is an initial number.
        /// </summary>
        INITIAL,

        /// <summary>
        /// Number is provided from the user.
        /// </summary>
        USER,

        /// <summary>
        /// Number is provided by the SudokuSolver.
        /// </summary>
        SOLVER,

        /// <summary>
        /// Square has no number (is empty).
        /// </summary>
        NONE
    }


    /// <summary>
    /// Interface for square-associated information.
    /// </summary>
    public interface ISquareInfo
    {
        /// <summary>
        /// Get number associated to the square. Number 0 implies empty square.
        /// </summary>
        /// <returns>Number associated to the square.</returns>
        int Number();

        /// <summary>
        /// Get candidates associated to the square by the user.
        /// </summary>
        /// <returns>Enumeration of candidates set to the square.</returns>
        IEnumerable<int> Candidates();

        /// <summary>
        /// Get source of number associated to the square.
        /// </summary>
        /// <returns>Number's source.</returns>
        SquareNumberSource NumberSource();
    }
}
