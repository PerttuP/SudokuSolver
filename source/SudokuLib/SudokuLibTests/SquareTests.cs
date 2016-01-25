using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SudokuLib.Tests
{
    /// <summary>
    /// Unit tests for the Square class.
    /// </summary>
    [TestClass()]
    public class SquareTests
    {
        /// <summary>
        /// Test the default constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareConstructor1Test()
        {
            Square sqr = new Square();
            Assert.AreEqual(0, sqr.Number);
            Assert.AreEqual(0, sqr.Candidates().Count());
        }


        /// <summary>
        /// Test the number-assigning constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareConstructor2Test()
        {
            for (int i=1; i<10; ++i)
            {
                Square sqr = new Square(i);
                Assert.AreEqual(i, sqr.Number);
                Assert.IsFalse(sqr.Empty());
                Assert.AreEqual(0, sqr.Candidates().Count());
            }
        }


        /// <summary>
        /// Tests the candidates-assigning constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareConstructor3Test()
        {
            List<int> cands = new List<int>();
            for (int i=1; i<10; ++i)
            {
                cands.Add(i);
                Square sqr = new Square(cands);

                Assert.IsTrue(sqr.Empty());
                Assert.AreEqual(cands.Count(), sqr.Candidates().Count());
                foreach (int n in sqr.Candidates())
                {
                    Assert.IsTrue(cands.Contains(n));
                }
            }
        }


        /// <summary>
        /// Tests setting number to the square.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareSetNumberTest()
        {
            Square sqr = new Square();
            Assert.IsTrue(sqr.Empty());

            for(int i=1; i<10; ++i)
            {
                sqr.Clear();
                sqr.InsertCandidate(i);
                Assert.IsTrue(sqr.Empty());
                Assert.IsTrue(sqr.HasCandidate(i));

                sqr.Number = i;
                Assert.IsFalse(sqr.Empty());
                Assert.AreEqual(i, sqr.Number);
                Assert.AreEqual(0, sqr.Candidates().Count());
            }
        }


        /// <summary>
        /// Tests the Empty method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareEmptyTest()
        {
            Square sqr = new Square();
            Assert.IsTrue(sqr.Empty());

            sqr.InsertCandidate(1);
            Assert.IsTrue(sqr.Empty());

            sqr.Number = 1;
            Assert.IsFalse(sqr.Empty());

            sqr.Clear();
            Assert.IsTrue(sqr.Empty());
        }


        /// <summary>
        /// Tests the Clear method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareClearTest()
        {
            Square sqr = new Square();
            Assert.IsTrue(sqr.Empty());
            Assert.AreEqual(0, sqr.Number);
            Assert.AreEqual(0, sqr.Candidates().Count());

            sqr.InsertCandidate(1);
            Assert.IsTrue(sqr.Empty());
            Assert.AreEqual(0, sqr.Number);
            Assert.AreNotEqual(0, sqr.Candidates().Count());

            sqr.Clear();
            Assert.IsTrue(sqr.Empty());
            Assert.AreEqual(0, sqr.Number);
            Assert.AreEqual(0, sqr.Candidates().Count());

            sqr.Number = 9;
            Assert.IsFalse(sqr.Empty());
            Assert.AreEqual(9, sqr.Number);
            Assert.AreEqual(0, sqr.Candidates().Count());

            sqr.Clear();
            Assert.IsTrue(sqr.Empty());
            Assert.AreEqual(0, sqr.Number);
            Assert.AreEqual(0, sqr.Candidates().Count());
        }


        /// <summary>
        /// Tests the HasCandidate method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareHasCandidateTest()
        {
            Square sqr = new Square();
            for (int i = 1; i < 10; ++i)
            {
                for (int j=i; j<10; ++j)
                {
                    Assert.IsFalse(sqr.HasCandidate(j));
                }
                sqr.InsertCandidate(i);
                for (int j=1; j<=i; ++j)
                {
                    Assert.IsTrue(sqr.HasCandidate(j));
                }
            }
        }


        /// <summary>
        /// Tests the RemoveCandidates method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareRemoveCandidateTest()
        {
            Square sqr = new Square();
            SortedSet<int> cands = new SortedSet<int>();
            for (int i=1; i<10; ++i)
            {
                cands.Add(i);
            }
            sqr.InsertCandidate(cands);

            for (int i=1; i<10; ++i)
            {
                for (int j=i; j<10; ++j)
                {
                    Assert.IsTrue(sqr.HasCandidate(j));
                }
                sqr.RemoveCandidate(i);
                for (int j=1; j<= i; ++j)
                {
                    Assert.IsFalse(sqr.HasCandidate(j));
                }
            }
            Assert.AreEqual(0, sqr.Candidates().Count());
        }


        /// <summary>
        /// Tests inserting a single candidate to the square.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareInsertCandidateTest()
        {
            Square sqr = new Square();
            for (int i = 1; i < 10; ++i)
            {
                for (int j = i; j < 10; ++j)
                {
                    Assert.IsFalse(sqr.HasCandidate(j));
                }
                sqr.InsertCandidate(i);
                for (int j = 1; j <= i; ++j)
                {
                    Assert.IsTrue(sqr.HasCandidate(j));
                }
            }
        }


        /// <summary>
        /// Tests inserting multiple candidates at once.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareInsertMultipleCandidateTest()
        {
            Square sqr = new Square();
            List<int> cands = new List<int>();
            for (int i=1; i<10; ++i)
            {
                cands.Add(i);
                sqr.InsertCandidate(cands);
                for (int j=1; j<=i; ++j)
                {
                    Assert.IsTrue(sqr.HasCandidate(j));
                }
                for (int j = i+1; j <= 10; ++j)
                {
                    Assert.IsFalse(sqr.HasCandidate(j));
                }
                sqr.Clear();
            }
        }


        /// <summary>
        /// Tests cloning.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void SquareCloneTest()
        {
            // Clone Empty square
            {
                Square s1 = new Square();
                Square s2 = (Square)s1.Clone();
                Assert.IsTrue(s1 != s2);
                Assert.AreEqual(s1.Number, s2.Number);
                Assert.AreEqual(s1.Candidates().Count(), s2.Candidates().Count());
                Assert.AreEqual(0, s1.Candidates().Count);
            }

            // Clone square having a assigned number.
            for (int i=1; i<10; ++i)
            {
                Square s1 = new Square(i);
                Square s2 = (Square)s1.Clone();
                Assert.IsTrue(s1 != s2);
                Assert.AreEqual(s1.Number, s2.Number);
                Assert.AreEqual(s1.Candidates().Count(), s2.Candidates().Count());
                Assert.AreEqual(0, s1.Candidates().Count);
            }

            // Clone square having candidates
            {
                Square s1 = new Square(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                Square s2 = (Square)s1.Clone();
                Assert.IsTrue(s1 != s2);
                Assert.AreEqual(s1.Number, s2.Number);
                Assert.AreEqual(0, s2.Number);
                Assert.AreEqual(s1.Candidates().Count(), s2.Candidates().Count());
                Assert.AreEqual(9, s1.Candidates().Count);
                for (int i=0; i<9; ++i)
                {
                    Assert.AreEqual(s1.Candidates().ElementAt(i), s2.Candidates().ElementAt(i));
                }
            }
        }
    }
}