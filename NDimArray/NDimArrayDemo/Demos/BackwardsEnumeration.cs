using NDimArray;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo.Demos
{
    class BackwardsEnumeration<T> : IDemo<T>
    {
        private NDimArray<T> _array;

        public NDimArray<T> Array
        {
            get { return _array; }
        }

        public BackwardsEnumeration(NDimArray<T> array)
        {
            _array = array;
        }

        public void Run(Action<int[], T> action)
        {
            Array.Enumerate(
                Array.GetUpperBoundaries(),
                Array.GetLowerBoundaries(),
                (index, item) => Console.WriteLine($"[{String.Join(", ", index)}]: {item}")
                );
        }
    }
}
