using NDimArray;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayTests
{
    [TestFixture]
    public class NDimArrayTests
    {
        #region Constructor NDimArray<T>(int[] lengths)
        [Test]
        public void Constructor_NDimArrayConstructedWithValidArray_ThrowsNoException()
        {
            int[] validArray = new int[] { 2, 2, 2 };

            Assert.DoesNotThrow(() => new NDimArray<string>(validArray));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithNegativeLength_ThrowsArgumentOutOfRangeException()
        {
            int[] negativeArray = new int[] { -3, 0, 2 };

            Assert.Throws<ArgumentOutOfRangeException>(() => new NDimArray<string>(negativeArray));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithEmptyArray_ThrowsArgumentException()
        {
            int[] lengths = new int[] { };

            Assert.Throws<ArgumentException>(() => new NDimArray<string>(lengths));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithNullLength_ThrowsArgumentNullException()
        {
            int[] nullArray = null;

            Assert.Throws<ArgumentNullException>(() => new NDimArray<string>(nullArray));
        }
        #endregion

        #region Constructor NDimArray<T>(int[] lengths, int[] lowerBounds)
        [Test]
        public void Constructor_NDimArrayConstructedWithValidBoundaries_ThrowsNoException()
        {
            int[] validArray = new int[] { 2, 2, 2 };
            int[] validBoundaries = new int[] { -1, -1, -1 };

            Assert.DoesNotThrow(() => new NDimArray<string>(validArray, validBoundaries));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithNullBounds_ThrowsArgumentNullException()
        {
            int[] validArray = new int[] { 2, 2, 2 };
            int[] nullArray = null;

            Assert.Throws<ArgumentNullException>(() => new NDimArray<string>(validArray, nullArray));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithEmptyBounds_ThrowsArgumentException()
        {
            int[] validArray = new int[] { 2, 2, 2 };
            int[] emptyArray = new int[] { };

            Assert.Throws<ArgumentException>(() => new NDimArray<string>(validArray, emptyArray));
        }

        [Test]
        public void Constructor_NDimArrayConstructedWithUnequalLengthParameters_ThrowsArgumentException()
        {
            int[] validArray = new int[] { 2, 2, 2 };
            int[] extraLowerIndices = new int[] { -1, -1, 0, 0 };

            Assert.Throws<ArgumentException>(() => new NDimArray<string>(validArray, extraLowerIndices));
        }
        #endregion

        #region Constructor NDimArray<T>(T[] array)
        [Test]
        public void Constructor_NDimArrayConstructedFromNullArray_ThrowsArgumentNullException()
        {
            string[] array = null;

            Assert.Throws<ArgumentNullException>(() => new NDimArray<string>(array));
        }

        [Test]
        public void Constructor_NDimArrayConstructedFromExistingArray_ThrowsNoException()
        {
            string[] array = new string[] { "a", "b", "c", "d", "e" };

            Assert.DoesNotThrow(() => new NDimArray<string>(array));
        }
        #endregion

        #region Properties
        [Test]
        [TestCase(new int[] { 2, 2, 2 }, ExpectedResult = 3)]
        [TestCase(new int[] { 2, 2 }, ExpectedResult = 2)]
        [TestCase(new int[] { 1 }, ExpectedResult = 1)]
        [TestCase(new int[] { 2, 2, 4, 6, 7 }, ExpectedResult = 5)]
        public int Rank_NDimArrayConstructed_RankCorrect(int[] lengths)
        {
            NDimArray<string> array = new NDimArray<string>(lengths);

            return array.Rank;
        }

        [Test]
        [TestCase(new int[] { 2, 2, 2 }, ExpectedResult = 8)]
        [TestCase(new int[] { 3, 4, 5 }, ExpectedResult = 60)]
        [TestCase(new int[] { 4, 10, 1 }, ExpectedResult = 40)]
        public int Length_NDimArrayConstructed_LengthCorrect(int[] lengths)
        {
            NDimArray<string> array = new NDimArray<string>(lengths);

            return array.Length;
        }

        [Test]
        public void GetIsReadOnly_NDimArrayConstructed_ReturnsFalse()
        {
            var strings = new string[] { "a", "b", "c", "d" };

            var array = new NDimArray<string>(strings);

            Assert.IsFalse(array.IsReadOnly);
        }

        [Test]
        public void GetIsSynchronized_NDimArrayConstructed_ReturnsFalse()
        {
            var strings = new string[] { "a", "b", "c", "d" };

            var array = new NDimArray<string>(strings);

            Assert.IsFalse(array.IsSynchronized);
        }

        [Test]
        public void GetIsFixedSize_NDimArrayConstructed_ReturnsTrue()
        {
            var strings = new string[] { "a", "b", "c", "d" };

            var array = new NDimArray<string>(strings);

            Assert.IsTrue(array.IsFixedSize);
        }
        #endregion

        #region Method Tests
        [Test]
        [TestCase(new int[] { -1, 2, 0 }, 1, ExpectedResult = 2)]
        [TestCase(new int[] { -1, 2, 0 }, 2, ExpectedResult = 0)]
        [TestCase(new int[] { -1, 2, 0 }, 0, ExpectedResult = -1)]
        public int GetLowerBound_ValidDimension_ReturnsCorrectUpperBoundary(int[] validLowerBounds, int validDim)
        {
            var lengths = (int[])Array.CreateInstance(typeof(int), validLowerBounds.Length);
            for (int i = 0; i < lengths.Length; i++)
            {
                lengths[i] = 2;
            }


            var array = new NDimArray<string>(lengths, validLowerBounds);

            return array.GetLowerBound(validDim);
        }

        [Test]
        public void GetLowerBound_InvalidDimension_ThrowsIndexOutOfRangeException()
        {
            var array = new NDimArray<string>(2, 2, 2);

            Assert.Throws<IndexOutOfRangeException>(() => array.GetLowerBound(array.Rank)); //gets the final dimension + 1, which should always be invalid
        }

        [Test]
        [TestCase(new int[] { 1, 2, 3 }, 1, ExpectedResult = 1)]
        [TestCase(new int[] { 2, 3, 4 }, 2, ExpectedResult = 3)]
        [TestCase(new int[] { 1 }, 0, ExpectedResult = 0)]
        public int GetUpperBound_ValidDimension_ReturnsCorrectUpperBoundary(int[] lengths, int validDim)
        {
            var array = new NDimArray<string>(lengths);

            return array.GetUpperBound(validDim);
        }

        [Test]
        public void GetUpperBound_InvalidDimension_ThrowsIndexOutOfRangeException()
        {
            var array = new NDimArray<string>(2, 2, 2);

            Assert.Throws<IndexOutOfRangeException>(() => array.GetUpperBound(array.Rank)); //gets the final dimension + 1, which should always be invalid
        }
        #endregion
    }
}
