using System.Diagnostics;
using SudokuLib;

namespace SolverLib
{
    /// <summary>
    /// Represents an action made by the solver. 
    /// </summary>
    public class SolverAction
    {
        private readonly Coordinate _loc;
        private readonly int _num;

        /// <summary>
        /// Constructs an invalid SolverAction object.
        /// </summary>
        public SolverAction()
        {
            _loc = null;
            _num = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location where number is set.</param>
        /// <param name="number">Number being set.</param>
        /// <remarks>
        /// Number must be in range [0,9]. Value 0 means clearing the square.
        /// </remarks>
        public SolverAction(Coordinate location, int number)
        {
            Debug.Assert(0 <= number && number < 10);
            Debug.Assert(location != null);
            _loc = location;
            _num = number;
        }


        /// <summary>
        /// Location where number is set.
        /// </summary>
        public Coordinate Location
        {
            get { return _loc; }
        }

        
        /// <summary>
        /// Number that has been set. Value 0 means clearing value.
        /// </summary>
        public int Number
        {
            get { return _num; }
        }


        /// <summary>
        /// Check if SolverAction is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return _loc != null;
        }
    }
}
