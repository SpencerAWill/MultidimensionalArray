using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public class NIndex : INIndex, IEnumerable<int>
    {
        private int[] _indices;
        public int[] Indices { get => _indices; }
        IReadOnlyList<int> INIndex.Indices { get => _indices; }

        public int this[int dimension]
        {
            get => _indices[dimension];
            set => _indices[dimension] = value;
        }

        public NIndex(params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices", "indices is null");
            if (indices.Length == 0)
                throw new ArgumentException("indices", "indices is empty");

            _indices = indices;
        }

        /// <summary>
        /// A - B
        /// </summary>
        public static int[] Difference(INIndex a, INIndex b)
        {
            if (a == null)
                throw new ArgumentNullException("a", "a is null");
            if (b == null)
                throw new ArgumentNullException("b", "b is null");
            if (a.Indices.Count != b.Indices.Count)
                throw new ArgumentException("b", "Dimensions of the indices are not equal");

            int[] diff = new int[a.Indices.Count];
            for (int i = 0; i < a.Indices.Count; i++)
            {
                diff[i] = a.Indices[i] - b.Indices[i];
            }
            return diff;
        }

        public static int[] Unit(IEnumerable<int> index)
        {
            if (index == null)
                throw new ArgumentNullException("index", "index is null");
            var count = index.Count();
            if (count == 0)
                throw new ArgumentException("index", "index cannot be empty");

            var retIndex = new int[count];

            for (int i = 0; i < count; i++)
            {
                var item = index.ElementAt(i);
                retIndex[i] = item == 0 ? 0 : item / Math.Abs(item);
            }

            return retIndex;
        }

        public static int[] Unit(INIndex index)
        {
            if (index == null)
                throw new ArgumentNullException("index", "index is null");

            return Unit(index.Indices);
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (var item in _indices)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public NIndex Origin() => 
            Origin(Indices.Length);

        public static NIndex Origin(int dimensions)
        {
            if (dimensions < 1)
                throw new ArgumentOutOfRangeException("dimensions", "dimensions must be greater than 0");

            return new NIndex((int[])Array.CreateInstance(typeof(int), dimensions));
        }
    }

    public interface INIndex : IEnumerable<int>
    {
        IReadOnlyList<int> Indices { get; }
        int this[int dimension] { get; }
    }
}
