using System;
using System.Collections.Generic;

namespace SudokuLib
{
    /// <summary>
    /// Abstract interface for sudoku table. 
    /// Offers abstraction for the concrete Table class.
    /// </summary>
    public interface ITable : ICloneable
    {
        /// <summary>
        /// Get number assigned to square in location pointed by given coordinate. 
        /// </summary>
        /// <param name="location">Square location.</param>
        /// <returns>Square's number, or 0 if square is empty.</returns>
        int NumberAt(Coordinate location);

        /// <summary>
        /// Check if square at given location is empty.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <returns>True, if square is empty.</returns>
        bool EmptyAt(Coordinate location);

        /// <summary>
        /// Get candidates of square at given location.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <returns>List of candidates.</returns>
        List<int> CandidatesAt(Coordinate location);

        /// <summary>
        /// Check if Table is ready (all squares are filled correctly).
        /// </summary>
        /// <returns>True, if table is ready.</returns>
        bool IsReady();

        /// <summary>
        /// Assign number to square at given location.
        /// </summary>
        /// <param name="location">Square's location.</param>
        /// <param name="number">Number to be assigned.</param>
        /// <exception cref="ConflictException">
        /// Thrown, if assignment causes a conflict (basic guarantee).
        /// </exception>
        /// <remarks>
        /// Number must be in range 0-9. 
        /// If number is 0, the square's current number is removed.
        /// </remarks>
        void SetNumber(Coordinate location, int number);

        /// <summary>
        /// Revert Table to its initial state.
        /// </summary>
        void Reset();

        /// <summary>
        /// Get table's square regions.
        /// </summary>
        /// <returns>SquareRegions representing the grouping.</returns>
        SquareRegions Regions();

        /// <summary>
        /// Get list of coordinates of squares that belong in same region 
        /// with given square.
        /// </summary>
        /// <param name="location">Reference square.</param>
        /// <returns>List of coordinates pointing to squares in same region.</returns>
        List<Coordinate> GetRegionOf(Coordinate location);
    }
}
