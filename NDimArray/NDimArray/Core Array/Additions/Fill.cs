using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArray
{
    public partial class NDimArray<T>
    {
        public static void Fill(NDimArray<T> array, IPath path, Func<INIndex, T, T> fillRule)
        {
            // null checks
            if (array == null)
                throw new ArgumentNullException("dimPriorities", "dimPriorities is null");
            if (path == null)
                throw new ArgumentNullException("path", "path is null");

            if (!array.ValidIndex(path.Start))
                throw new IndexOutOfRangeException("1 or more dimension indices in start was out of the range of the array");
            if (!array.ValidIndex(path.End))
                throw new IndexOutOfRangeException("1 or more dimension indices in end was out of the range of the array");

            array.Enumerate(path, (index, item) => array[index] = fillRule(index, item));
        }

        public static void Fill(NDimArray<T> array, IPath path, T value) =>
            Fill(array, path, (index, item) => value);

        public static void Fill(NDimArray<T> array, Func<INIndex, T, T> fillRule) =>
            Fill(
                array,
                new IndexPath(
                    new NIndex(array.GetLowerBoundaries()),
                    new NIndex(array.GetUpperBoundaries()),
                    EnumerationPriorities.CreateStandard(array.Rank)),
                fillRule);

        public static void Fill(NDimArray<T> array, T value) =>
            Fill(array, (index, item) => value);

    }
}
