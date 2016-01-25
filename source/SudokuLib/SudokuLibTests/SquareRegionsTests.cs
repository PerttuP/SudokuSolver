using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Tests
{
    /// <summary>
    /// Unit tests for the SquareRegions class.
    /// </summary>
    [TestClass()]
    public class SquareRegionsTests
    {
        // Test context for data driven tests.
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareRegionsConstructorTest()
        {
            SquareRegions regions = new SquareRegions();
            Assert.IsFalse(regions.IsValid());
            for (int i=1; i<10; ++i)
            {
                Assert.IsNull(regions.Region(i));
            }
        }


        /// <summary>
        /// Tests the IsValid method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        [DeploymentItem("TestData.xls")]
        [DataSource("SquareRegionsIsValidSource")]
        public void SquareRegionsIsValidTest()
        {
            // Fetch data from source
            bool valid = Convert.ToBoolean(TestContext.DataRow["valid"]);
            String regionsStr = Convert.ToString(TestContext.DataRow["region_string"]);

            // Construct regions from test data and set them to regions.
            SquareRegions regions = SquareRegions.Parse(regionsStr);
            
            Assert.AreEqual(valid, regions.IsValid());
        }

        /// <summary>
        /// Helper for IsValidTest. Converts region string into list of coordinates.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private IEnumerable<Coordinate> ParseRegion(string s)
        {
            List<Coordinate> rv = new List<Coordinate>();
            if (s.Length == 0) return rv;

            List<String> parts = new List<String>(s.Split(','));
            for (int i=0; i<parts.Count(); ++i)
            {
                char r = parts.ElementAt(i).ElementAt(0);
                char c = parts.ElementAt(i).ElementAt(1);
                int row = (int)Char.GetNumericValue(r);
                int col = (int)Char.GetNumericValue(c);
                Coordinate coord = new Coordinate(row, col);
                rv.Add(coord);
            }
            return rv;
        }


        /// <summary>
        /// Tests the Region method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareRegionsRegionTest()
        {
            // Create coordinates.
            SquareRegions regions = new SquareRegions();
            List<List<Coordinate>> coordinates = new List<List<Coordinate>>();
            for (int i=1; i<10; ++i)
            {
                coordinates.Add(new List<Coordinate>());
                for (int j=1; j<10; ++j)
                {
                    Coordinate c = new Coordinate(i, j);
                    coordinates[i - 1].Add(c);
                }
                regions.SetRegion(coordinates[i - 1], i);
            }

            for (int i=1; i<10; ++i)
            {
                foreach (Coordinate c in coordinates[i-1])
                {
                    Assert.IsTrue(regions.Region(i).Contains(c));
                }
            }
        }


        /// <summary>
        /// Tests cloning.
        /// </summary>
        [TestMethod]
        [Timeout(1000)]
        public void SquareRegionsCloneTest()
        {
            // Create coordinates.
            SquareRegions regions = new SquareRegions();
            List<List<Coordinate>> coordinates = new List<List<Coordinate>>();
            for (int i = 1; i < 10; ++i)
            {
                coordinates.Add(new List<Coordinate>());
                for (int j = 1; j < 10; ++j)
                {
                    Coordinate c = new Coordinate(i, j);
                    coordinates[i - 1].Add(c);
                }
                regions.SetRegion(coordinates[i - 1], i);
            }

            SquareRegions regionsClone = (SquareRegions)regions.Clone();

            // Verify equal regions.
            for (int i=1; i<10; ++i)
            {
                List<Coordinate> r1 = regions.Region(i);
                List<Coordinate> r2 = regionsClone.Region(i);
                Assert.IsFalse(r1 == r2);
                Assert.AreEqual(r1.Count, r2.Count);
                // Verify equal coordinates.
                for (int j=0; j<9; ++j)
                {
                    Assert.IsTrue(r1.ElementAt(j) != r2.ElementAt(j));
                    Assert.AreEqual(r1.ElementAt(j), r2.ElementAt(j));
                }
            }
        }


        /// <summary>
        /// Tests the IsEquivalent method.
        /// </summary>
        [TestMethod]
        [Timeout(1000)]
        public void SquareRegionsIsEquivalentTest()
        {
            SquareRegions regionsA = SquareRegions.DefaultRegions();
            SquareRegions regionsB = SquareRegions.DefaultRegions();
            Assert.IsTrue(regionsA != regionsB);
            Assert.IsTrue(regionsA.IsEquivalent(regionsB));
            Assert.IsTrue(regionsA.IsEquivalent(regionsA));
            Assert.IsFalse(regionsA.IsEquivalent(new SquareRegions()));
        }


        /// <summary>
        /// Tests the static DefaultRegions method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareRegionsDefaultRegionsTest()
        {
            List<List<Coordinate>> regions = createDefaultRegions();
            SquareRegions defaultRegions = SquareRegions.DefaultRegions();

            for (int i=1; i<10; ++i)
            {
                foreach (Coordinate c in regions[i - 1])
                {
                    Assert.IsTrue(defaultRegions.Region(i).Contains(c));
                }
            }
        }


        /// <summary>
        /// Helper for SquareRegionsDefaultRegionTest. 
        /// Create List of correct regions.
        /// </summary>
        /// <returns></returns>
        private List<List<Coordinate>> createDefaultRegions()
        {
            // Create list of correct coordinates.
            List<List<Coordinate>> regions = new List<List<Coordinate>>();
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(1,1), new Coordinate(1,2), new Coordinate(1,3),
                new Coordinate(2,1), new Coordinate(2,2), new Coordinate(2,3),
                new Coordinate(3,1), new Coordinate(3,2), new Coordinate(3,3)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(1,4), new Coordinate(1,5), new Coordinate(1,6),
                new Coordinate(2,4), new Coordinate(2,5), new Coordinate(2,6),
                new Coordinate(3,4), new Coordinate(3,5), new Coordinate(3,6)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(1,7), new Coordinate(1,8), new Coordinate(1,9),
                new Coordinate(2,7), new Coordinate(2,8), new Coordinate(2,9),
                new Coordinate(3,7), new Coordinate(3,8), new Coordinate(3,9)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(4,1), new Coordinate(4,2), new Coordinate(4,3),
                new Coordinate(5,1), new Coordinate(5,2), new Coordinate(5,3),
                new Coordinate(6,1), new Coordinate(6,2), new Coordinate(6,3)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(4,4), new Coordinate(4,5), new Coordinate(4,6),
                new Coordinate(5,4), new Coordinate(5,5), new Coordinate(5,6),
                new Coordinate(6,4), new Coordinate(6,5), new Coordinate(6,6)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(4,7), new Coordinate(4,8), new Coordinate(4,9),
                new Coordinate(5,7), new Coordinate(5,8), new Coordinate(5,9),
                new Coordinate(6,7), new Coordinate(6,8), new Coordinate(6,9)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(7,1), new Coordinate(7,2), new Coordinate(7,3),
                new Coordinate(8,1), new Coordinate(8,2), new Coordinate(8,3),
                new Coordinate(9,1), new Coordinate(9,2), new Coordinate(9,3)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(7,4), new Coordinate(7,5), new Coordinate(7,6),
                new Coordinate(8,4), new Coordinate(8,5), new Coordinate(8,6),
                new Coordinate(9,4), new Coordinate(9,5), new Coordinate(9,6)
            }
            ));
            regions.Add(new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(7,7), new Coordinate(7,8), new Coordinate(7,9),
                new Coordinate(8,7), new Coordinate(8,8), new Coordinate(8,9),
                new Coordinate(9,7), new Coordinate(9,8), new Coordinate(9,9)
            }
            ));
            return regions;
        }
    }
}