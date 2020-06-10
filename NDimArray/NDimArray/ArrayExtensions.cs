using System;
using System.Collections.Generic;
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
        private static int[] DimEnumerator(this Array array, Func<int, int> func)
        {
            var retArray = new int[array.Rank];

            for (int i = 0; i < retArray.Length; i++)
            {
                retArray[i] = func(i);
            }

            return retArray;
        }
    }
}
