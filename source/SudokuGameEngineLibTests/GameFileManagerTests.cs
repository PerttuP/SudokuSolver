using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuGameEngineLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuLib;

namespace SudokuGameEngineLib.Tests
{
    /// <summary>
    /// Unit tests for the GameFileManager class.
    /// </summary>
    [TestClass()]
    public class GameFileManagerTests
    {
        // TODO: Negative testing.

        // Test context for data driven tests.
        private TestContext _context;
        public TestContext TestContext
        {
            get { return _context; }
            set { _context = value; }
        }


        /// <summary>
        /// Positive tests for writing gamedata and reading it again.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void GameFileManagerReadWriteValidTest()
        {
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            IDictionary<Coordinate, int> initVals = ParseSudokuValues(initStr);
            IDictionary<Coordinate, int> readyVals = ParseSudokuValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);
            Dictionary<Coordinate, int> userVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> solverVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, List<int>> candidates = new Dictionary<Coordinate, List<int>>();

            foreach (Coordinate c in initVals.Keys)
            {
                readyVals.Remove(c);
            }
            int counter = 0;
            foreach (KeyValuePair<Coordinate, int> pair in readyVals)
            {
                if (counter % 2 == 0)
                {
                    solverVals.Add(pair.Key, pair.Value);
                }
                else
                {
                    userVals.Add(pair.Key, pair.Value);
                }
                ++counter;
            }

            GameData data = new GameData(initVals, userVals, solverVals, candidates, layout);
            GameFileManager manager = new GameFileManager();
            manager.WriteGame("tmpfile.xml", data);
            GameData data2 = manager.ReadGame("tmpfile.xml");
            Assert.IsNotNull(data2);
            Assert.IsTrue(data2.IsValid());
            CompareGameData(data, data2);
        }


        public static IDictionary<Coordinate,int> ParseSudokuValues(string s)
        {
            Dictionary<Coordinate, int> rv = new Dictionary<Coordinate, int>();
            List<String> rows = new List<String>(s.Split(','));
            for (int row=0; row<rows.Count; ++row)
            {
                for (int col = 0; col < 9; ++col)
                {
                    Coordinate c = new Coordinate(row + 1, col + 1);
                    int num = (int)Char.GetNumericValue(rows[row][col]);
                    if (num != 0)
                    {
                        rv.Add(c, num);
                    }
                }
            }
            return rv;
        }


        private void CompareGameData(GameData lhs, GameData rhs)
        {
            Assert.IsTrue(lhs.Layout.IsEquivalent(rhs.Layout));
            Assert.AreEqual(lhs.InitialValues.Count, rhs.InitialValues.Count);
            Assert.AreEqual(lhs.UserValues.Count, rhs.UserValues.Count);
            Assert.AreEqual(lhs.SolverValues.Count, rhs.SolverValues.Count);
            Assert.AreEqual(lhs.Candidates.Count, rhs.Candidates.Count);

            foreach (KeyValuePair<Coordinate,int> pair in lhs.InitialValues)
            {
                Assert.IsTrue(rhs.InitialValues.Contains(pair));
            }
            foreach (KeyValuePair<Coordinate, int> pair in lhs.UserValues)
            {
                Assert.IsTrue(rhs.UserValues.Contains(pair));
            }
            foreach (KeyValuePair<Coordinate, int> pair in lhs.SolverValues)
            {
                Assert.IsTrue(rhs.SolverValues.Contains(pair));
            }

            foreach (KeyValuePair<Coordinate, List<int>> pair in lhs.Candidates){
                Assert.IsTrue(rhs.Candidates.ContainsKey(pair.Key));
                List<int> rhsList = rhs.Candidates[pair.Key];
                Assert.AreEqual(pair.Value.Count, rhsList.Count);
                foreach (int n in rhsList)
                {
                    Assert.IsTrue(pair.Value.Contains(n));
                }
            }
        }
    }
}