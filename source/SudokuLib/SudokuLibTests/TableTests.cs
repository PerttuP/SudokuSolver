using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Tests
{
    /// <summary>
    /// Unit tests for the Table class.
    /// </summary>
    [TestClass()]
    public class TableTests
    {
        // Test context for data-driven tests.
        private TestContext context;
        public TestContext TestContext
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Test the non-parametrized constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(10000)]
        public void TableConstructor1Test()
        {
            Table t = new Table();

            // Check that squares are empty, and have all candidates.
            List<int> allCands = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    Assert.IsTrue( t.EmptyAt(c) );
                    List<int> cands = t.CandidatesAt(c);
                    foreach (int cnd in allCands)
                    {
                        Assert.IsTrue(cands.Contains(cnd));
                    }
                }
            }

            // Check that the regions are the default.
            SquareRegions layout = t.Regions();
            SquareRegions expected = SquareRegions.DefaultRegions();
            Assert.IsNotNull(layout);
            Assert.IsTrue(expected.IsEquivalent(layout));
        }


        /// <summary>
        /// Test the parametrized constructor with valid initial values.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestValidSource")]
        public void TableConstructor2ValidTest()
        {
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            SquareRegions regions = SquareRegions.Parse(layoutStr);
            Table t = new Table(initVals, regions);

            foreach (KeyValuePair<Coordinate,int> pair in initVals)
            {
                Assert.AreEqual(pair.Value, t.NumberAt(pair.Key));
            }
            this.CheckCandidates(t);
        }


        /// <summary>
        /// Test the parametrized constructor with invalid initial values.
        /// </summary>
        [TestMethod]
        [Timeout(10000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestInvalidSource")]
        public void TableConstructor2InvalidTest()
        {
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            SquareRegions regions = SquareRegions.Parse(layoutStr);

            try
            {
                Table t = new Table(initVals, regions);
            }
            catch (ConflictException)
            {
                // OK
            }
            catch (Exception)
            {
                Assert.Fail("Unknown exception!");
            }
        }


        /// <summary>
        /// Helper method. Checks that all candidates in table are as expected.
        /// </summary>
        /// <param name="t"></param>
        private void CheckCandidates(Table t)
        {
            SquareRegions regions = t.Regions();
            for (int i=1; i<10; ++i)
            {
                List<Coordinate> region = regions.Region(i);

                // Assigned values in region
                List<int> assignedReg = new List<int>();
                foreach (Coordinate c in region)
                {
                    int num = t.NumberAt(c);
                    if (num != 0)
                    {
                        assignedReg.Add(num);
                    }
                }

                foreach (Coordinate c in region)
                {
                    List<int> assignedRow = new List<int>();
                    List<int> assignedCol = new List<int>();
                    for (int j=1; j<10; ++j)
                    {
                        Coordinate onRow = new Coordinate(c.Row, j);
                        Coordinate onCol = new Coordinate(j, c.Column);
                        if (!t.EmptyAt(onRow))
                            assignedRow.Add(t.NumberAt(onRow));
                        if (!t.EmptyAt(onCol))
                            assignedCol.Add(t.NumberAt(onCol));
                    }
                    // Check that square candidates do not contain any assigned
                    // values, and has all the others
                    for (int num = 1; num<10; ++num)
                    {
                        if (assignedReg.Contains(num) ||
                            assignedRow.Contains(num) ||
                            assignedCol.Contains(num))
                        {
                            Assert.IsFalse(t.CandidatesAt(c).Contains(num));
                        }
                        else if (t.EmptyAt(c))
                            Assert.IsTrue(t.CandidatesAt(c).Contains(num));
                    }
                }
            }
        }


        /// <summary>
        /// Test the IsReady-method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestValidSource")]
        public void TableIsReadyTest()
        {
            string initialString = Convert.ToString(TestContext.DataRow["initial"]);
            string finalString = Convert.ToString(TestContext.DataRow["finished"]);
            string layoutString = Convert.ToString(TestContext.DataRow["layout"]);
            SquareRegions regions = SquareRegions.Parse(layoutString);

            // Not ready table
            Dictionary<Coordinate, int> initDict = ParseInitialValues(initialString);
            Table tNotReady = new Table(initDict, regions);
            Assert.IsFalse(tNotReady.IsReady());

            // Ready table
            Dictionary<Coordinate, int> readyDict = ParseInitialValues(finalString);
            Table tReady = new Table(readyDict, regions);
            Assert.IsTrue(tReady.IsReady());
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
            List<String> rows = new List<string>( initialString.Split(',') );

            for (int i=0; i<rows.Count; ++i)
            {
                for (int j=0; j<rows[i].Length; ++j)
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


        /// <summary>
        /// Test setting numbers to squares in table.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TableSetNumberTest()
        {
            Table t = new Table();
            // Set numbers 1-9 on top row.
            for (int i = 1; i < 10; ++i)
            {
                Coordinate cr = new Coordinate(1, i);
                t.SetNumber(cr, i);
            }

            // Verify correct numbers and candidates.
            // Numbers
            for (int i = 1; i < 10; ++i)
            {
                Coordinate cr = new Coordinate(1, i);
                Assert.AreEqual(i, t.NumberAt(cr));
            }
            // Candidates in first row of regions
            for (int row = 1; row < 4; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    int baseNum = (int)(Math.Ceiling(col / 3.0) - 1) * 3;
                    for (int n = baseNum+1; n < baseNum+4; ++n)
                    {
                        Assert.IsFalse(t.CandidatesAt(c).Contains(n));
                    }
                }
            }
            // Candidates in squares not belonging to same region with set values.
            for (int row = 4; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    for (int n = 1; n < 10; ++n)
                    {
                        if (n == col)
                        {
                            Assert.IsFalse(t.CandidatesAt(c).Contains(n));
                        }
                        else
                        {
                            Assert.IsTrue(t.CandidatesAt(c).Contains(n));
                        }
                    }
                }
            }

            // Create conflict.
            try
            {
                t.SetNumber(new Coordinate(9, 9), 9);
            }
            catch (ConflictException)
            {
                // OK.
            }
            catch (Exception)
            {
                Assert.Fail("Unknown exception.");
            }
        }


        /// <summary>
        /// Test the Reset-method.
        /// </summary>
        [TestMethod()]
        [Timeout(10000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestValidSource")]
        public void TableResetTest()
        {
            // Fetch test data.
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            // Process test data.
            Dictionary<Coordinate,int> initVals = ParseInitialValues(initStr);
            Dictionary<Coordinate, int> addVals = ParseInitialValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);
            foreach (Coordinate k in initVals.Keys)
            {
                addVals.Remove(k);
            }
            Table t = new Table(initVals, layout);

            // Solve table
            foreach (KeyValuePair<Coordinate,int> pair in addVals)
            {
                t.SetNumber(pair.Key, pair.Value);
                this.CheckCandidates(t);
            }
            Assert.IsTrue(t.IsReady());

            // Revert
            t.Reset();
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    if (initVals.ContainsKey(c))
                    {
                        Assert.AreEqual(initVals[c], t.NumberAt(c));
                    }
                    else
                    {
                        Assert.IsTrue(t.EmptyAt(c));
                    }
                }
            }
            CheckCandidates(t);
        }


        /// <summary>
        /// Test cloning.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestValidSource")]
        public void TableCloneTest()
        {
            string initStr = Convert.ToString(TestContext.DataRow["initial"]);
            string layoutStr = Convert.ToString(TestContext.DataRow["layout"]);
            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            SquareRegions regions = SquareRegions.Parse(layoutStr);

            Table t1 = new Table(initVals, regions);
            Table t2 = (Table)t1.Clone();

            Assert.IsTrue(t1.Regions().IsEquivalent(t2.Regions()));
            for (int row=1; row<10; ++row)
            {
                for (int col=1; col<10; ++col)
                {
                    Coordinate c = new Coordinate(row, col);
                    Assert.AreEqual(t1.NumberAt(c), t2.NumberAt(c));
                    List<int> cands1 = t1.CandidatesAt(c);
                    List<int> cands2 = t2.CandidatesAt(c);
                    Assert.AreEqual(cands1.Count, cands2.Count);
                    foreach (int n in cands1)
                    {
                        Assert.IsTrue(cands2.Contains(n));
                    }
                }
            }
        }


        /// <summary>
        /// Test clearing squares in table.
        /// </summary>
        [TestMethod()]
        [Timeout(10000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("TableTestValidSource")]
        public void TableRemoveNumbersTest()
        {
            String initStr = Convert.ToString(TestContext.DataRow["initial"]);
            String readyStr = Convert.ToString(TestContext.DataRow["finished"]);
            String layoutStr = Convert.ToString(TestContext.DataRow["layout"]);

            Dictionary<Coordinate, int> initVals = ParseInitialValues(initStr);
            Dictionary<Coordinate, int> addVals = ParseInitialValues(readyStr);
            SquareRegions layout = SquareRegions.Parse(layoutStr);
            foreach (Coordinate c in initVals.Keys)
            {
                addVals.Remove(c);
            }

            Table t = new Table(initVals, layout);
            CheckCandidates(t);
            // Fill table
            foreach (KeyValuePair<Coordinate, int> pair in addVals)
            {
                t.SetNumber(pair.Key, pair.Value);
                this.CheckCandidates(t);
            }
            Assert.IsTrue(t.IsReady());

            // Remove filled values.
            foreach (Coordinate c in addVals.Keys)
            {
                t.SetNumber(c, 0);
            }
            Assert.IsFalse(t.IsReady());
            CheckCandidates(t);
        }
    }
}