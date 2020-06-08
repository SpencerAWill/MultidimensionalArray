using NDimArray;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo.Demos
{
    public class RegularEnumeration<T> : IDemo<T>
    {
        private NDimArray<T> _array;

        public NDimArray<T> Array
        {
            get { return _array; }
        }

        public RegularEnumeration(NDimArray<T> array)
        {
            _array = array;
        }

        public void Run(Action<int[], T> action)
        {
            Array.Enumerate(
                (index, item) => { action(index, item); } 
                );
        }
    }
}
