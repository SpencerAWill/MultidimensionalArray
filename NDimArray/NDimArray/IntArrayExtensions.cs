using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public static class IntArrayExtensions
    {
        internal static int[] SubtractEach(this int[] array, int[] other)
        {
            return array.IndivFunc(other, (a, b) => a - b);
        }

        internal static int[] DivideEach(this int[] dividends, int[] divisors)
        {
            return dividends.IndivFunc(divisors, (a, b) => (int)(a / b));
        }

        internal static int[] IndivFunc(this int[] a, int[] b, Func<int, int, int> func)
        {
            if (a.Length != b.Length)
                throw new ArgumentOutOfRangeException("other", "other array must be the same rank and length as this array.");

            int[] returnArray = new int[a.GetLength(0)];

            for (int i = 0; i < a.Length; i++)
            {
                returnArray[i] = func(a[i], b[i]);
            }

            return returnArray;
        }

        internal static bool ElementsUnique(this int[] array)
        {
            List<int> frequencies = new List<int>(array.Length);
            foreach (var item in array)
            {
                if (frequencies.Contains(item))
                    return false;
                else
                    frequencies.Add(item);
            }
            return true;
        }

        internal static bool ElementsUniqueAlt<T>(this T[] array)
        {
            return !array.SequenceEqual(array.Distinct());
        }
    }
}
