using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace SudokuLib.Tests
{
    /// <summary>
    /// Unit tests for the SudokuLib.Coordinate class.
    /// </summary>
    [TestClass()]
    public class CoordinateTests
    {
        /// <summary>
        /// Test the constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void CoordinateConstructionTest()
        {
            List<int> invalidParam = new List<int>(new int[] { 0, 10 });

            for (int r=1; r<10; ++r)
            {
                for (int c=1; c<10; ++c)
                {
                    Coordinate coord = new Coordinate(r, c);
                    Assert.AreEqual(r, coord.Row);
                    Assert.AreEqual(c, coord.Column);
                }
                foreach (int c in invalidParam)
                {
                    try
                    {
                        Coordinate coord = new Coordinate(r, c);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Assert.AreEqual("column", e.ParamName);
                        Assert.IsTrue( e.Message.Contains("Column must be in range [1,9].") );
                    }
                }
            }

            foreach (int r in invalidParam)
            {
                for (int c=1; c<10; ++c)
                {
                    try
                    {
                        Coordinate coord = new Coordinate(r, c);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Assert.AreEqual("row", e.ParamName);
                        Assert.IsTrue( e.Message.Contains("Row must be in range [1,9].") );
                    }
                }
            }
            

        }


        /// <summary>
        /// Test cloning.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void CoordinateCloneTest()
        {
            for (int r=1; r<10; ++r)
            {
                for (int c=1; c<10; ++c)
                {
                    Coordinate coord1 = new Coordinate(r, c);
                    Coordinate coord2 = (Coordinate)coord1.Clone();
                    Assert.AreEqual(coord1.Row, coord2.Row);
                    Assert.AreEqual(coord1.Column, coord2.Column);
                }
            }
        }


        /// <summary>
        /// Test equality comparator.
        /// </summary>
        [TestMethod]
        [Timeout(1000)]
        public void CoordinateEqualityTest()
        {
            for (int r1=1; r1<10; ++r1)
            {
                for (int c1=1; c1<10; ++c1)
                {
                    Coordinate coord1 = new Coordinate(r1, c1);
                    for (int r2=1; r2<10; ++r2)
                    {
                        for (int c2 = 1; c2 < 10; ++c2)
                        {
                            Coordinate coord2 = new Coordinate(r2, c2);
                            bool eq = coord1.Row == coord2.Row && coord1.Column == coord2.Column;
                            Assert.AreEqual(eq, coord1.Equals(coord2));
                        }
                    }
                }
            }
        }


        [TestMethod()]
        [Timeout(1000)]
        public void CoordinateToStringTest()
        {
            // Coordinate to string and back to coordinate.
            for (int row = 1; row < 10; ++row)
            {
                for (int col = 1; col < 10; ++col)
                {
                    Coordinate c1 = new Coordinate(row, col);
                    string s1 = c1.ToString();
                    Coordinate c2 = Coordinate.Parse(s1);
                    Assert.AreEqual(c1, c2);
                    Assert.AreEqual(s1, c2.ToString());
                }
            }
        }
    }
}