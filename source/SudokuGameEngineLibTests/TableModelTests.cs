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
    [TestClass()]
    public class TableModelTests
    {
        // TODO: Negative testing.

        private TestContext _context;
        public TestContext TestContext
        {
            get { return _context; }
            set { _context = value; }
        }

        [TestMethod()]
        [Timeout(1000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void TableModelConstructorPositiveTest()
        {
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            IDictionary<Coordinate, int> initVals = GameFileManagerTests.ParseSudokuValues(initStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);

            TableModel model = new TableModel(initVals, layout);

            Assert.IsFalse(model.IsReady());
            Assert.IsTrue(layout.IsEquivalent(model.Layout()));
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    ISquareInfo info = model.Info(c);
                    if (initVals.ContainsKey(c))
                    {
                        Assert.AreEqual(0, info.Candidates().Count());
                        Assert.AreEqual(initVals[c], info.Number());
                        Assert.AreEqual(SquareNumberSource.INITIAL, info.NumberSource());
                    }
                    else
                    {
                        Assert.AreEqual(0, info.Candidates().Count());
                        Assert.AreEqual(0, info.Number());
                        Assert.AreEqual(SquareNumberSource.NONE, info.NumberSource());
                    }
                }
            }
        }



        [TestMethod()]
        [Timeout(1000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void TableModelSetNumberTest()
        {
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            IDictionary<Coordinate, int> initVals = GameFileManagerTests.ParseSudokuValues(initStr);
            IDictionary<Coordinate, int> addVals = GameFileManagerTests.ParseSudokuValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);

            foreach (Coordinate c in initVals.Keys)
            {
                addVals.Remove(c);
            }

            // Set and remove values in model.
            TableModel model = new TableModel(initVals, layout);
            int counter = 0;
            foreach (KeyValuePair<Coordinate, int> pair in addVals)
            {
                // Set number
                SquareNumberSource source = SquareNumberSource.NONE;
                if (counter % 2 == 0)
                {
                    source = SquareNumberSource.SOLVER;
                }
                else
                {
                    source = SquareNumberSource.USER;
                }
                model.SetNumber(pair.Key, pair.Value, source);
                ISquareInfo info = model.Info(pair.Key);
                Assert.AreEqual(0, info.Candidates().Count());
                Assert.AreEqual(pair.Value, info.Number());
                Assert.AreEqual(source, info.NumberSource());

                // Remove number.
                model.SetNumber(pair.Key, 0, SquareNumberSource.NONE);
                info = model.Info(pair.Key);
                Assert.AreEqual(0, info.Number());
                Assert.AreEqual(0, info.Candidates().Count());
                Assert.AreEqual(SquareNumberSource.NONE, info.NumberSource());

                ++counter;
            }
        }


        [TestMethod()]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableValidSource")]
        public void TableModelAddCandidateTest()
        {
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            IDictionary<Coordinate, int> initVals = GameFileManagerTests.ParseSudokuValues(initStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);

            TableModel model = new TableModel(initVals, layout);

            int counter = 0;
            for (int row=1; row<10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (!initVals.ContainsKey(c))
                    {
                        // Add candidate.
                        ++counter;
                        int cand = counter % 9 + 1;
                        model.AddCandidate(c, cand);
                        ISquareInfo info = model.Info(c);
                        Assert.AreEqual(0, info.Number());
                        Assert.AreEqual(SquareNumberSource.NONE, info.NumberSource());
                        Assert.AreEqual(1, info.Candidates().Count());
                        Assert.IsTrue(info.Candidates().Contains(cand));

                        // Remove candidate
                        model.RemoveCandidate(c, cand);
                        info = model.Info(c);
                        Assert.AreEqual(0, info.Number());
                        Assert.AreEqual(SquareNumberSource.NONE, info.NumberSource());
                        Assert.AreEqual(0, info.Candidates().Count());
                    }
                }
            }
        }
    }
}