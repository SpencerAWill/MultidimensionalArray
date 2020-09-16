using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo
{
    public static class ArrayExtensions
    {
        public static long Product(this int[] array)
        {
            var cumProd = 1;
            for (int i = 0; i < array.Length; i++)
            {
                cumProd *= array[i];
            }
            return cumProd;
        }
    }
}
