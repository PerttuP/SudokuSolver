using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLib;
using SolverLib;

namespace SudokuGameEngineLib
{
    internal class TableModel : ISudokuTableReadonlyModel
    {
        private Dictionary<Coordinate, SquareInfo> _infos;
        private ITable _readyTable;
        private ITable _currentTable;
        private bool _finished;


        /// <summary>
        /// Implements the ISudokuTableReadonlyModel.
        /// </summary>
        /// <param name="location">Square location.</param>
        /// <returns>Information about the square.</returns>
        public ISquareInfo Info(Coordinate location)
        {
            return _infos[location];
        }


        public SquareRegions Layout()
        {
            return _currentTable.Regions();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initVals">Initial coordinate-number pairs.</param>
        /// <param name="layout">SudokuTable's layout.</param>
        /// <exception cref="ConflictException">
        /// Thrown, if parameters do not represent a solvable Sudoku table.
        /// </exception>
        public TableModel(IDictionary <Coordinate,int> initVals, SquareRegions layout)
        {
            _finished = false;
            _readyTable = TableFactory.Create(initVals, layout);
            _currentTable = (ITable)_readyTable.Clone();

            ISudokuSolver solver = SolverFactory.SimpleSolver();
            if (!solver.Solve(ref _readyTable))
            {
                throw new ConflictException();
            }

            _infos = new Dictionary<Coordinate, SquareInfo>();
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (initVals.ContainsKey(c))
                    {
                        _infos.Add(c, new SquareInfo(initVals[c], SquareNumberSource.INITIAL));
                    }
                    else
                    {
                        _infos.Add(c, new SquareInfo());
                    }
                }
            }
        }


        /// <summary>
        /// Check if current table is finished.
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return _finished;
        }


        /// <summary>
        /// Set or remove number in given location.
        /// </summary>
        /// <param name="loc">Location.</param>
        /// <param name="num">Number. if num=0, current number is removed.</param>
        public void SetNumber(Coordinate loc, int num, SquareNumberSource src)
        {
            if (_finished) return;
            
            _infos[loc].SetNumber(num, src);

            if (_readyTable.NumberAt(loc) == num)
            {
                _currentTable.SetNumber(loc, num);
            }
            else if (!_currentTable.EmptyAt(loc))
            {
                _currentTable.SetNumber(loc, 0);
            }

            _finished = _currentTable.IsReady();
        }


        /// <summary>
        /// Add candidate to the given location.
        /// </summary>
        /// <param name="loc">Location.</param>
        /// <param name="candidate">Candidate.</param>
        public void AddCandidate(Coordinate loc, int candidate)
        {
            if (_infos.ContainsKey(loc))
            {
                _infos[loc].AddCandidate(candidate);
            }
            else if (!_finished)
            {
                _infos.Add(loc, new SquareInfo(new int[] { candidate }));
            }
        }


        /// <summary>
        /// Remove candidate from given location.
        /// </summary>
        /// <param name="loc">Location.</param>
        /// <param name="candidate">Candidate to be removed.</param>
        public void RemoveCandidate(Coordinate loc, int candidate)
        {
            if (_infos.ContainsKey(loc))
            {
                _infos[loc].RemoveCandidate(candidate);
            }
        }


        /// <summary>
        /// Fix one error or solve next number.
        /// </summary>
        public void Hint()
        {
            // Check if current table has errors.
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (_infos[c].NumberSource() != SquareNumberSource.NONE &&
                        _infos[c].Number() != _readyTable.NumberAt(c))
                    {
                        _infos[c].SetNumber(_readyTable.NumberAt(c), SquareNumberSource.SOLVER);
                        _currentTable.SetNumber(c, _readyTable.NumberAt(c));
                        _finished = _currentTable.IsReady();
                        return;
                    }
                }
            }

            // Solve next.
            SolverAction a = SolverFactory.SimpleSolver().NextAction(_currentTable);
            if (a.IsValid())
            {
                _infos[a.Location].SetNumber(a.Number, SquareNumberSource.SOLVER);
                _currentTable.SetNumber(a.Location, a.Number);
                _finished = _currentTable.IsReady();
            }
        }


        /// <summary>
        /// Solve table.
        /// </summary>
        public void Solve()
        {
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (_infos[c].Number() == 0 || 
                        _infos[c].Number() != _readyTable.NumberAt(c))
                    {
                        _infos[c].SetNumber(_readyTable.NumberAt(c), SquareNumberSource.SOLVER);
                    }
                }
            }
            _finished = true;
        }


        /// <summary>
        /// Revert table back to initial numbers.
        /// </summary>
        public void Reset()
        {
            for (int row=1; row<10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (_infos[c].NumberSource() != SquareNumberSource.INITIAL)
                    {
                        _infos[c].SetNumber(0, SquareNumberSource.NONE);
                    }
                }
            }
            _currentTable.Reset();
            _finished = false;
        }


        public GameData GameData()
        {
            Dictionary<Coordinate, int> initVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> userVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> solverVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, List<int>> candidates = new Dictionary<Coordinate, List<int>>();

            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    SquareNumberSource src = _infos[c].NumberSource();
                    switch (src)
                    {
                        case SquareNumberSource.INITIAL:
                            initVals.Add(c, _infos[c].Number());
                            break;
                        case SquareNumberSource.USER:
                            userVals.Add(c, _infos[c].Number());
                            break;
                        case SquareNumberSource.SOLVER:
                            solverVals.Add(c, _infos[c].Number());
                            break;
                        case SquareNumberSource.NONE:
                            if (_infos[c].Candidates().Count() != 0)
                            {
                                candidates.Add(c, new List<int>(_infos[c].Candidates()));
                            }
                            break;
                    }
                }
            }

            return new GameData(initVals, userVals, solverVals, candidates, _readyTable.Regions());
        }
    }
}
