using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gets an array of the first indices in each dimension.
        /// </summary>
        internal static int[] GetLowerBoundaries(this Array array) =>
            DimEnumerator(array, x => array.GetLowerBound(x));

        /// <summary>
        /// Gets an array of the last indices in each dimension.
        /// </summary>
        internal static int[] GetUpperBoundaries(this Array array) =>
            DimEnumerator(array, x => array.GetUpperBound(x));

        /// <summary>
        /// Function that acts on every dimension of an array.
        /// </summary>
        /// <param name="func">Takes in an indexed dimension.</param>
        internal static int[] DimEnumerator(this Array array, Func<int, int> func)
        {
            var retArray = new int[array.Rank];

            for (int i = 0; i < retArray.Length; i++)
            {
                retArray[i] = func(i);
            }

            return retArray;
        }

        internal static int[] Reduce(this int[] array) => 
            array.Select(i => i == 0 ? 0 : i / Math.Abs(i)).ToArray();

        internal static int[] Difference(this int[] array, int[] other)
        {
            _ = other ?? throw new ArgumentNullException(nameof(other));

            if (array.Length != other.Length)
                throw new ArgumentException($"{nameof(array)} must be the same length as the length of {nameof(other)}");

            int[] ret = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                ret[i] = array[i] - other[i];
            }
            return ret;
        }
    }
}
