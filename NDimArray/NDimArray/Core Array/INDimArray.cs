using System;
using System.Collections.Generic;

namespace NDimArray
{
    public interface INDimArray<T> : IEnumerable<T>, IEnumerable<Tuple<INIndex, T>>
    {
        int Rank { get; }
        int Length { get; }
        object SyncRoot { get; }
        bool IsFixedSize { get; }
        bool IsSynchronized { get; }
        bool IsReadOnly { get; }

        T this[params int[] index] { get; }
        T this[INIndex index] { get; }
        T GetValue(params int[] index);
        T GetValue(INIndex index);

        int GetLength(int dimension);
        int GetLowerBound(int dimension);
        int GetUpperBound(int dimension);
    }

    public interface IReadOnlyNDimArray<T> : INDimArray<T>
    {
        new bool IsReadOnly { get; }
    }
}
