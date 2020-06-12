using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArray
{
    public class NIndex : INIndex
    {
        private int[] _indices;

        public IReadOnlyList<int> Indices { get => _indices; }

        public int this[int dimension]
        {
            get => Indices[dimension];
            private set => _indices[dimension] = value;
        }

        public NIndex(int[] indices)
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
    }

    public interface INIndex
    {
        IReadOnlyList<int> Indices { get; }
    }
}
