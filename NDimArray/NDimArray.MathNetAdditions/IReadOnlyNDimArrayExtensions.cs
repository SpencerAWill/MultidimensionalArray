using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray.MathNetAdditions
{
    public static class IReadOnlyNDimArrayExtensions
    {
        public static bool ValidIndex<T>(this IReadOnlyNDimArray<T> array, Vector<int> index)
        {
            _ = index ?? throw new ArgumentNullException(nameof(index));

            if (array.Rank != index.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "index count must equal the rank of the array");

            for (int i = 0; i < index.Count; i++)
            {
                if (index[i] < array.GetLowerBound(i) || index[i] > array.GetUpperBound(i))
                    return false;
            }
            return true;
        }

        public static T GetValue<T>(this IReadOnlyNDimArray<T> array, Vector<int> index)
        {
            _ = index ?? throw new ArgumentNullException(nameof(index));

            if (array.ValidIndex(index))
                return array.GetValue(index.ToArray<int>());
            else
                throw new ArgumentException($"{nameof(array)} must be the same rank as the length of the {nameof(index)} vector");
        }
    }
}
