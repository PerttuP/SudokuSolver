using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Class holds information about square that is important for the UI.
    /// </summary>
    internal class SquareInfo : ISquareInfo
    {

        private int _number;
        private SortedSet<int> _candidates;
        private SquareNumberSource _src;

        /// <summary>
        /// Constructs SquareInfo with no number or candidates.
        /// </summary>
        public SquareInfo()
        {
            _number = 0;
            _candidates = new SortedSet<int>();
            _src = SquareNumberSource.NONE;
        }

        /// <summary>
        /// Constructs SquareInfo with number and its source.
        /// </summary>
        /// <param name="num">Number, must be in range [1,9].</param>
        /// <param name="src">Number's source. Should not be Source.NONE.</param>
        public SquareInfo(int num, SquareNumberSource src)
        {
            Debug.Assert(src != SquareNumberSource.NONE && num > 0 && num < 10);
            _src = src;
            _number = num;
            _candidates = new SortedSet<int>();
        }

        /// <summary>
        /// Constructs SquareInfo representing an empty square.
        /// </summary>
        /// <param name="candidates">Square's candidates. All elements must be in range [1,9].</param>
        public SquareInfo(IEnumerable<int> candidates)
        {
            foreach (int n in candidates)
            {
                Debug.Assert(n>0 && n<10);
            }
            _src = SquareNumberSource.NONE;
            _number = 0;
            _candidates = new SortedSet<int>(candidates);
        }


        /// <summary>
        /// Add candidates.
        /// </summary>
        /// <param name="cand">New candidate.</param>
        /// <remarks>
        /// Number must be 0 when calling this method. cand must be in range [1,9].
        /// </remarks>
        public void AddCandidate(int cand)
        {
            Debug.Assert(_number == 0);
            Debug.Assert(cand > 0 && cand < 10);
            _candidates.Add(cand);
        }


        /// <summary>
        /// IRemove candidates.
        /// </summary>
        /// <param name="cand"></param>
        public void RemoveCandidate(int cand)
        {
            _candidates.Remove(cand);
        }


        /// <summary>
        /// Setter for the number.
        /// </summary>
        /// <param name="num">Number.</param>
        /// <param name="src">Number's source.</param>
        /// <remarks>
        /// Number must be in range [0,9], 0 meaning removing number.
        /// if num == 0, then src must be Source.None. Else it has to be something else.
        /// </remarks>
        internal void SetNumber(int num, SquareNumberSource src)
        {
            Debug.Assert(num >= 0 && num < 10);
            Debug.Assert(num == 0 ? src == SquareNumberSource.NONE : src != SquareNumberSource.NONE);
            _candidates.Clear();
            _number = num;
            _src = src;
        }

        /// <summary>
        /// Implements the ISquareInfo interface.
        /// </summary>
        /// <returns>Number associated to the square.</returns>
        public int Number()
        {
            return _number;
        }

        /// <summary>
        /// Implements the ISquareInfo interface.
        /// </summary>
        /// <returns>Candidates associated to the square by the user.</returns>
        public IEnumerable<int> Candidates()
        {
            return _candidates.AsEnumerable();
        }

        /// <summary>
        /// Implements the ISquareInfo interface.
        /// </summary>
        /// <returns>Source of square's number.</returns>
        public SquareNumberSource NumberSource()
        {
            return _src;
        }
    }
}
