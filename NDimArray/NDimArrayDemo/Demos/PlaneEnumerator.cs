﻿using NDimArray;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDimArrayDemo.Demos
{
    public class PlaneEnumerator<T> : IDemo<T>
    {
        private NDimArray<T> _array;

        public NDimArray<T> Array
        {
            get { return _array; }
        }

        public PlaneEnumerator(NDimArray<T> array)
        {
            _array = array;
        }

        public void Run(Action<int[], T> action)
        {
            Array.Enumerate(
                new EnumerationPath(
                    new int[] { 0, 0, 0 },
                    new int[] { 1, 0, 1 },
                    new int[] { 2, 1, 0 }), //in this case, because we are not going to moving through dimension 1, this functions more like a { 2, 0 } priority list)
                (index, item) => { Console.WriteLine($"[{String.Join(", ", index)}]: { item }"); } //action on each item
                );
        }
    }
}
