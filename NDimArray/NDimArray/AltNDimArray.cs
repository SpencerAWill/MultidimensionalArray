using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NDimArray
{
    public class AltNDimArray<T> : IEnumerable<T>
    {
        private T[] _items;

        protected T[] Items { get => _items; }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    internal class AltNDimArrayEnumerator<T> : IEnumerator<T>
    {
        private T[] _items;
        private int[] _lowerBounds;
        private int[] _upperBounds;
        private int[] _start;
        private int[] _end;

        private long _currentIndex;
        public T Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public AltNDimArrayEnumerator(T[] items, int[] lowerBounds, int[] upperBounds, int[] start, int[] end)
        {
            _items = items;
            _lowerBounds = lowerBounds;
            _upperBounds = upperBounds;
            _start = start;
            _end = end;
            Reset();
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }
}
