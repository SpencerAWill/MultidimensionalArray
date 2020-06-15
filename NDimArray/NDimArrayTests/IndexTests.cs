using NDimArray;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace NDimArrayTests
{
    [TestFixture]
    class IndexTests
    {
        class FakeIndex : INIndex
        {
            private int[] _indices;
            public int this[int dimension] => Indices[dimension];

            public IReadOnlyList<int> Indices => _indices;

            public FakeIndex(int[] uncheckedIndex) =>
                _indices = uncheckedIndex;

            public IEnumerator<int> GetEnumerator()
            {
                foreach (var item in Indices)
                    yield return item;
            }

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();
        }

        [Test]
        public void Constructor_IndexConstructedWithValidArray_ThrowsNoException()
        {
            int[] validIndices = new int[] { 2, 2, 2 };

            Assert.DoesNotThrow(() => new NIndex(validIndices));
        }

        [Test]
        public void Constructor_IndexConstructedWithNullArray_ThrowsArgumentNullException()
        {
            int[] nullIndices = null;

            Assert.Throws<ArgumentNullException>(() => new NIndex(nullIndices));
        }

        [Test]
        public void Constructor_IndexConstructedWithEmptyArray_ThrowsArgumentException()
        {
            int[] emptyIndices = new int[] { };

            Assert.Throws<ArgumentException>(() => new NIndex(emptyIndices));
        }



        [Test]
        public void Difference_ValidNIndices_ThrowsNoException()
        {
            NIndex n1 = new NIndex(new int[] { 2, 2, 2 });
            NIndex n2 = new NIndex(new int[] { 3, 3, 3 });

            var diff = NIndex.Difference(n2, n1);

            Assert.That(diff.SequenceEqual(new int[] { 1, 1, 1 }));
        }

        [Test]
        public void Difference_NullAIndex_ThrowsArgumentNullException()
        {
            NIndex n1 = null;
            NIndex n2 = new NIndex(new int[] { 3, 3, 3 });

            Assert.Throws<ArgumentNullException>(() => NIndex.Difference(n2, n1));
        }

        [Test]
        public void Difference_NullBIndex_ThrowsArgumentNullException()
        {
            NIndex n1 = new NIndex(new int[] { 2, 2, 2 });
            NIndex n2 = null;

            Assert.Throws<ArgumentNullException>(() => NIndex.Difference(n2, n1));
        }

        [Test]
        public void Difference_EmptyAIndex_ThrowsArgumentException()
        {
            INIndex a = new FakeIndex(new int[] { });
            INIndex b = new FakeIndex(new int[] { 2, 2, 2 });

            Assert.Throws<ArgumentException>(() => NIndex.Difference(a, b));
        }

        [Test]
        public void Difference_EmptyBIndex_ThrowsArgumentException()
        {
            INIndex a = new FakeIndex(new int[] { 2, 2, 2 });
            INIndex b = new FakeIndex(new int[] { });

            Assert.Throws<ArgumentException>(() => NIndex.Difference(a, b));
        }

        [Test]
        public void Unit_ValidIndex_ThrowsNoException()
        {
            NIndex index = new NIndex(new int[] { 3, 0, -3 });

            Assert.DoesNotThrow(() => NIndex.Unit(index));
        }

        [Test]
        public void Unit_NullIndex_ThrowsArgumentNullException()
        {
            NIndex index = null;

            Assert.Throws<ArgumentNullException>(() => NIndex.Unit(index));
        }

        [Test]
        public void Unit_EmptyIndex_ThrowsArgumentNullException()
        {
            INIndex index = new FakeIndex(new int[] { });

            Assert.Throws<ArgumentException>(() => NIndex.Unit(index));
        }
    }
}
