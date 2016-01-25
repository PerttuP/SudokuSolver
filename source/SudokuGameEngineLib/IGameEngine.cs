using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLib;


namespace SudokuGameEngineLib
{

    /// <summary>
    /// Interface for communicating with game logic from GUI.
    /// </summary>
    public interface IGameEngine
    {
        /// <summary>
        /// Start game described by given file.
        /// </summary>
        /// <param name="fileName">Xml-file name containing game data.</param>
        /// <returns>True, if game was started successfully.</returns>
        /// <remarks>If returns false, more info can be received form LastError().</remarks>
        bool ReadGameFromFile(string fileName);

        /// <summary>
        /// Save game to given file.
        /// </summary>
        /// <param name="fileName">Xml file name where game is saved to.</param>
        /// <returns>True, if game was saved successfully.</returns>
        /// <remarks>If returns false, more info can be received form LastError().</remarks>
        bool SaveGame(string fileName);

        /// <summary>
        /// Create new game from scrach.
        /// </summary>
        /// <param name="initVals">SudokuTable's initial coordinate-number pairs.</param>
        /// <param name="layout">Division in square regions.</param>
        /// <returns>True, if given values are valid and game was successfully created.</returns>
        /// <remarks>If returns false, more info can be received form LastError().</remarks>
        bool CreateGame(IDictionary<Coordinate, int> initVals, SquareRegions layout = null);

        /// <summary>
        /// Let SudokuSolver solve the game.
        /// </summary>
        /// <returns>True, if game was solved successfully.</returns>
        /// <remarks>If returns false, more info can be received form LastError().</remarks>
        bool Solve();

        /// <summary>
        /// Let SudokuSolver give a hint.
        /// </summary>
        /// <returns>True, if hint could have been given.</returns>
        /// <remarks>If returns false, more info can be received form LastError().</remarks>
        bool Hint();

        /// <summary>
        /// Insert number from the user.
        /// </summary>
        /// <param name="loc">Location</param>
        /// <param name="val">Number</param>
        /// <remarks>Val == 0 means removing current value.</remarks>
        void SetValue(Coordinate loc, int val);

        /// <summary>
        /// Set candidate from the user.
        /// </summary>
        /// <param name="loc">Location.</param>
        /// <param name="cand">Candidate to be added.</param>
        void SetCandidate(Coordinate loc, int cand);

        /// <summary>
        /// Remove candidate given from the user.
        /// </summary>
        /// <param name="loc">Location.</param>
        /// <param name="cand">Candidate to be removed.</param>
        void RemoveCandidate(Coordinate loc, int cand);

        /// <summary>
        /// Check if the user has solved the game.
        /// </summary>
        /// <returns>True, if game is solved.</returns>
        bool IsFinished();

        /// <summary>
        /// Revert game to the initial state.
        /// </summary>
        void ResetGame();

        /// <summary>
        /// Register ui to the game engine.
        /// </summary>
        /// <param name="ui">User interface.</param>
        void SetUI(IGameUI ui);

        /// <summary>
        /// Get information from latest error.
        /// </summary>
        /// <returns>Error describing latest error.</returns>
        IGameError LastError();
    }
}
