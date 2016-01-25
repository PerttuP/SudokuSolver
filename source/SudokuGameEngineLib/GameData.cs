using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLib;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Represents key information that will be stored in a xml-file
    /// when game is saved.
    /// </summary>
    internal class GameData
    {
        private readonly IDictionary<Coordinate, int> _initVals;
        private readonly IDictionary<Coordinate, int> _userVals;
        private readonly IDictionary<Coordinate, int> _solverVals;
        private readonly SquareRegions _layout;
        private readonly TimeSpan _elapsedTime;

        /// <summary>
        /// Construct an invalid GameData.
        /// </summary>
        public GameData()
        {
            _initVals = null;
            _userVals = null;
            _solverVals = null;
            _layout = null;
            _elapsedTime = new TimeSpan();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initVals">Initial coordinate-number pairs.</param>
        /// <param name="userVals">Coordinate-number pairs provided by the user.</param>
        /// <param name="solverVals">Coordinate-number pairs provided by the SudokuSolver.</param>
        /// <param name="elapsedTime">Time elapsed since game was started.</param>
        /// <param name="layout">Square regions. If null, default regions are used.</param>
        public GameData(
            IDictionary<Coordinate,int> initVals,
            IDictionary<Coordinate,int> userVals,
            IDictionary<Coordinate,int> solverVals,
            TimeSpan elapsedTime,
            SquareRegions layout
            )
        {
            Debug.Assert(initVals != null);
            Debug.Assert(userVals != null);
            Debug.Assert(solverVals != null);
            _initVals = initVals;
            _userVals = userVals;
            _solverVals = solverVals;
            _elapsedTime = elapsedTime;
            _layout = layout;

        }

        /// <summary>
        /// Initial coordinate-number pairs.
        /// </summary>
        public IDictionary<Coordinate,int> InitialValues
        {
            get { return _initVals; }
        }

        /// <summary>
        /// Coordinate-number pairs provided by the user.
        /// </summary>
        public IDictionary<Coordinate,int> UserValues
        {
            get { return _userVals; }
        }

        /// <summary>
        /// Coordinate-number pairs provided by the SudokuSolver.
        /// </summary>
        public IDictionary<Coordinate,int> SolverValues
        {
            get { return _solverVals; }
        }

        /// <summary>
        /// Time elapsed since game was started.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
        }

        /// <summary>
        /// Square regions.
        /// </summary>
        public SquareRegions Layout
        {
            get { return _layout; }
        }

        /// <summary>
        /// Check if GameData is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return _initVals != null && _solverVals != null && _userVals != null;
        }
    }
}
