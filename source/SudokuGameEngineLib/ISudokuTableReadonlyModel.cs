using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Model of sudoku table visible to the UI.
    /// </summary>
    public interface ISudokuTableReadonlyModel
    {
        /// <summary>
        /// Get information about square in given location.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <returns>SquareInfo representing requested square.</returns>
        ISquareInfo Info(SudokuLib.Coordinate location);

        /// <summary>
        /// Get table layout.
        /// </summary>
        /// <returns>SquareRegions describing layout.</returns>
        SudokuLib.SquareRegions Layout();
    }
}
