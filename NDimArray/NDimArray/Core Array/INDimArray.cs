using System;
using System.Collections.Generic;

namespace NDimArray
{
    public interface IReadOnlyNDimArray<T> : 
        IEnumerable<T>, 
        IEnumerable<Tuple<int[], T>>
    {
        int Rank { get; }
        int Length { get; }
        object SyncRoot { get; }
        bool IsFixedSize { get; }
        bool IsSynchronized { get; }
        bool IsReadOnly { get; }

        T this[params int[] index] { get; }
        T GetValue(params int[] index);
        bool ValidIndex(params int[] index);
        int GetLength(int dimension);
        int GetLowerBound(int dimension);
        int GetUpperBound(int dimension);
    }

    public interface INDimArray<T> : IReadOnlyNDimArray<T>
    {
        new T this[params int[] index] { get; set; }
        new bool IsReadOnly { get; }
        void SetValue(T value, params int[] index);
    }
}
