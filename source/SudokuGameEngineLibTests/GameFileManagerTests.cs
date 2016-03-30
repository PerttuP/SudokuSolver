using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuGameEngineLib;
using System;
using System.Collections.Generic;
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
        [Timeout(2000)]
        public void GameFileManagerReadWriteValidTest()
        {
            // Read test data from source.
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            IDictionary<Coordinate, int> initVals = ParseSudokuValues(initStr);
            IDictionary<Coordinate, int> readyVals = ParseSudokuValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);
            Dictionary<Coordinate, int> userVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> solverVals = new Dictionary<Coordinate, int>();

            // Share non-initial squares between EMPTY, USER and SOLVER types.
            foreach (Coordinate c in initVals.Keys)
            {
                readyVals.Remove(c);
            }
            int counter = 0;
            foreach (KeyValuePair<Coordinate, int> pair in readyVals)
            {
                if (counter % 3 == 0)
                {
                    solverVals.Add(pair.Key, pair.Value);
                }
                else if (counter % 3 == 1)
                {
                    userVals.Add(pair.Key, pair.Value);
                }
                // Else empty.
                ++counter;
            }

            // Get candidates from empty squares.
            IDictionary<Coordinate, List<int>> candidates = getCandidatesForEmpty(initVals, userVals, solverVals, layout);

            // Create GameData, write it to file and read it back
            GameData data = new GameData(initVals, userVals, solverVals, candidates, layout);
            GameFileManager manager = new GameFileManager();
            manager.WriteGame("tmpfile.xml", data);
            GameData data2 = manager.ReadGame("tmpfile.xml");
            
            // Verify
            Assert.IsNotNull(data2);
            Assert.IsTrue(data2.IsValid());
            CompareGameData(data, data2);
        }


        /// <summary>
        /// Test reading invalid game data files.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("TestData.xls")]
        [DeploymentItem("empty_number.xml")]
        [DeploymentItem("invalid_candidate.xml")]
        [DeploymentItem("invalid_layout.xml")]
        [DeploymentItem("missing_root.xml")]
        [DeploymentItem("non_empty_0.xml")]
        [DeploymentItem("syntax_error.xml")]
        [DataSource("InvalidGameFileSource")]
        [Timeout(2000)]
        public void GameFileManagerReadInvalidTest()
        {
            // TODO: Does not work yet due to missing test data.
            string fileName = Convert.ToString(TestContext.DataRow["fileName"]);

            GameFileManager manager = new GameFileManager();
            GameData d = manager.ReadGame(fileName);
            Assert.IsFalse(d.IsValid());
        }


        /// <summary>
        /// Helper method for parsing Coordinate-int dictionaries from string.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <returns>Dictionary parsed from the string.</returns>
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


        /// <summary>
        /// Helper method for comparing two GameData objects for equivalency.
        /// Causes test to fail if GameData objects are not equivalent.
        /// </summary>
        /// <param name="lhs">Expected GameData.</param>
        /// <param name="rhs">Actula GameData</param>
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


        /// <summary>
        /// Helper method for deducing candidates for empty squares.
        /// </summary>
        /// <param name="initVals"></param> Tables initial values.
        /// <param name="userVals"></param> Values provided by the user.
        /// <param name="solverVals"></param> Values provided by the solver.
        /// <param name="layout"></param> Table's layout.
        /// <returns>Dictionary of Coordinate - List of candidates -pairs.</returns>
        private IDictionary<Coordinate, List<int>> getCandidatesForEmpty(
            IDictionary<Coordinate, int> initVals,                                                            
            IDictionary<Coordinate, int> userVals,         
            IDictionary<Coordinate, int> solverVals,
            SquareRegions layout)
        {
            ITable t = TableFactory.Create(initVals, layout);
            foreach (KeyValuePair<Coordinate, int> pair in userVals)
            {
                t.SetNumber(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<Coordinate, int> pair in solverVals)
            {
                t.SetNumber(pair.Key, pair.Value);
            }

            Dictionary<Coordinate, List<int>> rv = new Dictionary<Coordinate, List<int>>();
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (t.EmptyAt(c))
                    {
                        rv.Add(c, t.CandidatesAt(c));
                    }
                }
            }
            return rv;
        }
    }
}