using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLib;

namespace SudokuGameEngineLib
{
    internal class GameEngine : IGameEngine
    {
        private IGameUI _ui;
        private TableModel _table;

        public GameEngine()
        {
            _ui = null;
            _table = null;
        }


        public void SetUI(IGameUI ui)
        {
            _ui = ui;
            _ui.setTableModel(_table);
        }


        public bool CreateGame(IDictionary<Coordinate, int> initVals, SquareRegions layout = null)
        {
            if (layout == null)
            {
                layout = SquareRegions.DefaultRegions();
            }
            else if (!layout.IsValid())
            {
                return false;
            }

            try
            {
                _table = new TableModel(initVals, layout);
                _ui.setTableModel(_table);
                return true;
            }
            catch (ConflictException)
            {
                return false;
            }
        }


        public bool Hint()
        {
            _table.Hint();
            return true;
        }


        public bool IsFinished()
        {
            return _table.IsReady();
        }


        public IGameError LastError()
        {
            throw new NotImplementedException();
        }


        public bool ReadGameFromFile(string fileName)
        {
            try
            {
                GameFileManager manager = new GameFileManager();
                GameData data = manager.ReadGame(fileName);
                if (data.IsValid())
                {
                    return false;
                }
                _table = new TableModel(data.InitialValues, data.Layout);
                foreach (KeyValuePair<Coordinate,int> pair in data.UserValues)
                {
                    _table.SetNumber(pair.Key, pair.Value, SquareNumberSource.USER);
                }
                foreach (KeyValuePair<Coordinate,int> pair in data.SolverValues)
                {
                    _table.SetNumber(pair.Key, pair.Value, SquareNumberSource.SOLVER);
                }
                foreach (KeyValuePair<Coordinate,List<int>> pair in data.Candidates)
                {
                    foreach (int c in pair.Value)
                    {
                        _table.AddCandidate(pair.Key, c);
                    }
                }

                _ui.setTableModel(_table);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public void RemoveCandidate(Coordinate loc, int cand)
        {
            _table.RemoveCandidate(loc, cand);
        }


        public bool SaveGame(string fileName)
        {
            GameData data = _table.GameData();
            GameFileManager manager = new GameFileManager();
            return manager.WriteGame(fileName, data);
        }

        public void SetCandidate(Coordinate loc, int cand)
        {
            _table.AddCandidate(loc, cand);
        }

        public void SetValue(Coordinate loc, int val)
        {
            _table.SetNumber(loc, val, SquareNumberSource.USER);
        }

        public bool Solve()
        {
            _table.Solve();
            return true;
        }

        public void ResetGame()
        {
            _table.Reset();
        }
    }
}
