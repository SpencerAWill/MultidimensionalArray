using NDimArray;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo
{
    public static class NDimArrayExtensions
    {
        public static int[] GetLowerBoundaries<T>(this NDimArray<T> array)
        {
            var ret = new int[array.Rank];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = array.GetLowerBound(i);
            }
            return ret;
        }

        public static int[] GetUpperBoundaries<T>(this NDimArray<T> array)
        {
            var ret = new int[array.Rank];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = array.GetUpperBound(i);
            }
            return ret;
        }
    }
}
