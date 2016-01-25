using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SudokuLib.Tests
{
    /// <summary>
    /// Unit tests for the SudokuLib.Group class.
    /// </summary>
    [TestClass()]
    public class GroupTests
    {

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void GroupConstructorTest()
        {
            // Create squares
            List<Square> sqrs = new List<Square>();
            for (int i = 0; i < 9; ++i)
            {
                Square sqr = new Square(i + 1);
                sqrs.Add(sqr);
            }
            Group grp = new Group(sqrs);

            // Do simple tests on group to verify that it was constructed properly.
            grp.ReduceCandidates();
            Assert.IsFalse(grp.CheckConflict());
        }


        /// <summary>
        /// Test constructor with invalid parameters.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void GroupInvalidConstructorTest()
        {
            List<Square> squares = new List<Square>();

            // Too few squares.
            for (int i=1; i<10; ++i)
            {
                try
                {
                    Group g = new Group(squares);
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual("squares", e.ParamName);
                    Assert.IsTrue( e.Message.Contains("Group must have exactly 9 members.") );
                }
                squares.Add(new Square(i));
            }

            // Too many squares
            squares.Add(new Square());
            try
            {
                Group g = new Group(squares);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("squares", e.ParamName);
                Assert.IsTrue(e.Message.Contains("Group must have exactly 9 members."));
            }

            // Duplicates.
            squares.RemoveAt(9);
            squares[0] = squares[1];
            try
            {
                Group g = new Group(squares);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("squares", e.ParamName);
                Assert.IsTrue(e.Message.Contains("All squares in group must be different objects."));
            }

        }

        /// <summary>
        /// Tests the ReduceCandidates method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void GroupReduceCandidatesTest()
        {
            // Create squares
            List<Square> sqrs = new List<Square>();
            for (int i = 0; i < 9; ++i)
            {
                Square sqr = new Square(i + 1);
                sqrs.Add(sqr);
            }
            Group grp = new Group(sqrs);

            // Reduction should have no effect on non-empty squares.
            grp.ReduceCandidates();
            Assert.IsFalse(grp.CheckConflict());
            foreach (Square s in sqrs)
            {
                Assert.IsFalse(s.Empty());
                Assert.AreEqual(0, s.Candidates().Count());
            }

            // Reduce single numbers
            for (int i=1; i<10; ++i)
            {
                List<int> allCands = new List<int>(new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                foreach (Square s in sqrs)
                {
                    s.Clear();
                    s.InsertCandidate(allCands);
                }
                sqrs.ElementAt(0).Number = i;
                grp.ReduceCandidates();

                Assert.IsFalse(grp.CheckConflict());
                foreach (Square s in sqrs)
                {
                    if (s.Empty())
                    {
                        Assert.AreEqual(8, s.Candidates().Count());
                    }
                    else
                    {
                        Assert.AreEqual(0, s.Candidates().Count());
                    }
                    Assert.IsFalse(s.Candidates().Contains(i));
                }
            }
        }


        /// <summary>
        /// Tests the CheckConflict method.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void GroupCheckConflictTest()
        {
            List<Square> sqrs = new List<Square>();
            for (int i=1; i<10; ++i)
            {
                Square s = new Square();
                sqrs.Add(s);
            }
            Group grp = new Group(sqrs);

            // Empty squares with no candidates count as conflict.
            Assert.IsTrue(grp.CheckConflict());
            
            // No conflict here
            foreach (Square s in sqrs)
            {
                s.InsertCandidate(new List<int>(new int[]{ 1,2,3,4,5,6,7,8,9}));
            }
            Assert.IsFalse(grp.CheckConflict());

            // An unassigned number without suitable square is a conflict.
            foreach (Square s in sqrs)
            {
                s.RemoveCandidate(9);
            }
            Assert.IsTrue(grp.CheckConflict());

            // No conflict now.
            sqrs.ElementAt(0).Number = 9;
            Assert.IsFalse(grp.CheckConflict());

            // Dublicate numbers.
            sqrs.ElementAt(1).Number = 9;
            Assert.IsTrue(grp.CheckConflict());
        }
    }
}