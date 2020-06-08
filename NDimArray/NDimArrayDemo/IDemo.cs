using NDimArray;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo
{
    public interface IDemo<T>
    {
        NDimArray<T> Array { get; }
        void Run(Action<int[], T> action);
    }
}
