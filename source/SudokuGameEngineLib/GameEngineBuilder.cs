using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Builder class for the concrete IGameEngine object.
    /// </summary>
    public class GameEngineBuilder
    {
        /// <summary>
        /// Create new game engine.
        /// </summary>
        /// <returns></returns>
        public IGameEngine Create()
        {
            return new GameEngine();
        }
    }
}
