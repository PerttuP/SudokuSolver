using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Interface for sudoku game errors.
    /// </summary>
    public interface IGameError
    {
        string Message();
    }
}
