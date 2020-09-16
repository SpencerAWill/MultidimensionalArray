using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray.MathNetAdditions
{
    public static class INDimArrayExtensions
    {
        public static void SetValue<T>(this INDimArray<T> array, T value, Vector<int> index)
        {
            if (array.ValidIndex(index))
                array.SetValue(value, index.ToArray<int>());
        }
    }
}
