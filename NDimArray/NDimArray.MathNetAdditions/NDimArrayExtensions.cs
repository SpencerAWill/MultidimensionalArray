using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDimArray.MathNetAdditions
{
    public static class NDimArrayExtensions
    {
        public static void SetValue<T>(this NDimArray<T> array, T value, Vector<int> index)
        {
            _ = index ?? throw new ArgumentNullException(nameof(index));

            array.SetValue(value, index.ToArray<int>());
        }

        public static void Enumerate<T>(this NDimArray<T> array, Action<Vector<int>, T> action)
        {
            array.Enumerate((array, item) =>
            {
                var vec = Vector<int>.Build.DenseOfEnumerable(array);
                action(vec, item);
            });
        }

        public static void Enumerate<T>(this NDimArray<T> array, IPath path, Action<Vector<int>, T> action)
        {
            array.Enumerate(path, (array, item) =>
            {
                var vec = Vector<int>.Build.DenseOfEnumerable(array);
                action(vec, item);
            });
        }
    }
}
