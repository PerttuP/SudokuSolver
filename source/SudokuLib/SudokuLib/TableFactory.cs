using System.Collections.Generic;

namespace SudokuLib
{
    /// <summary>
    /// Factory class for ITable objects.
    /// </summary>
    public class TableFactory
    {
        /// <summary>
        /// Get default-constructed table.
        /// </summary>
        /// <returns>New ITable object.</returns>
        public static ITable Create()
        {
            return new Table();
        }


        /// <summary>
        /// Get new ITable object.
        /// </summary>
        /// <param name="initVals">Initial values for new table.</param>
        /// <param name="layout">Table's layout. if null, default layout is used.</param>
        /// <returns>new ITable object.</returns>
        public static ITable Create (
            IDictionary<Coordinate, int> initVals,
            SquareRegions layout = null)
        {
            try
            {
                return new Table(initVals, layout);
            }
            catch (ConflictException)
            {
                return null;
            }
        }
    }
}
