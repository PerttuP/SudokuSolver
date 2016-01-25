using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using SudokuLib;

namespace SolverLib.Tests
{
    /// <summary>
    /// Tests the SimpleSolver sudoku solver.
    /// </summary>
    [TestClass()]
    public class SimpleSolverTests
    {
        // Test context for data driven tests.
        private TestContext _context;
        public TestContext TestContext
        {
            get { return _context; }
            set { _context = value; }
        }


        /// <summary>
        ///  Tests solving solvable tables.
        /// </summary>
        [TestMethod()]
        [Timeout(10000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void SimpleSolverSolveValidTest()
        {
            // Fetch test data.
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            Dictionary<Coordinate, int> readyVals = ParseInitialValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);

            // Create objects
            ITable t = TableFactory.Create(initVals, layout);
            SimpleSolver solver = new SimpleSolver();

            // Verify results.
            bool result = solver.Solve(ref t);
            Assert.IsTrue(result);
            Assert.IsTrue(t.IsReady());
            foreach (KeyValuePair<Coordinate,int> pair in readyVals)
            {
                Assert.AreEqual(pair.Value, t.NumberAt(pair.Key));
            }
        }



        [TestMethod()]
        //[Timeout(5000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void SimpleSolverNextActionValidTest()
        {
            // Fetch test data
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            // Create objects
            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            Dictionary<Coordinate, int> readyVals = ParseInitialValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);
            ITable t = TableFactory.Create(initVals, layout);
            SimpleSolver solver = new SimpleSolver();

            // Solve table one number at a time.
            while (!t.IsReady())
            {
                SolverAction a = solver.NextAction(t);
                Assert.IsTrue(a.IsValid());
                Assert.IsTrue(readyVals.ContainsKey(a.Location));
                Assert.AreEqual(readyVals[a.Location], a.Number);
                t.SetNumber(a.Location, a.Number);
            }

            // Verify results.
            foreach (KeyValuePair<Coordinate,int> pair in readyVals)
            {
                Assert.AreEqual(pair.Value, t.NumberAt(pair.Key));
            }
        }


        /// <summary>
        /// Creates dictionary of coordinate-number pairs out of 
        /// string read from test data table.
        /// </summary>
        /// <param name="initialString">String of square numbers.</param>
        /// <returns>Dictionary of coordinate-number pairs.</returns>
        private Dictionary<Coordinate, int> ParseInitialValues(string initialString)
        {
            Dictionary<Coordinate, int> rv = new Dictionary<Coordinate, int>();
            List<String> rows = new List<string>(initialString.Split(','));

            for (int i = 0; i < rows.Count; ++i)
            {
                for (int j = 0; j < rows[i].Length; ++j)
                {
                    int num = (int)Char.GetNumericValue(rows[i].ElementAt(j));
                    if (num != 0)
                    {
                        Coordinate c = new Coordinate(i + 1, j + 1);
                        rv.Add(c, num);
                    }
                }
            }

            return rv;
        }
    }
}