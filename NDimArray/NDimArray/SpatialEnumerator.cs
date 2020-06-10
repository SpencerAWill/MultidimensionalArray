using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    internal sealed partial class SpatialEnumerator<T> : IEnumerator<T>, IEnumerator<Tuple<int[], T>>
    {
        readonly Array array;
        bool firstEvaluated;

        readonly int[] _diff;
        readonly int[] _enumerationDirections;

        readonly int[] _startIndices;
        readonly int[] _endIndices;

        readonly int[] _dimPriorityList;
        int[] _currentIndex;

        public T Current => 
            (T)array.GetValue(_currentIndex);

        Tuple<int[], T> IEnumerator<Tuple<int[], T>>.Current => 
            new Tuple<int[], T>(_currentIndex, Current);

        object IEnumerator.Current => Current;

        public SpatialEnumerator(Array array, int[] startIndex, int[] endIndex, int[] enumPriorityList)
        {
            if (array == null)
                throw new ArgumentNullException("array", "array is null");

            if (startIndex.SequenceEqual(endIndex))
                throw new ArgumentException("indices are sequentially the same");

            this.array = array;

            ValidateConstructor(array, startIndex, endIndex, enumPriorityList);

            this.array = array;
            _startIndices = startIndex;
            _endIndices = endIndex;
            _dimPriorityList = enumPriorityList;

            _diff = _endIndices.SubtractEach(_startIndices); //gets a vector-like difference between the start and end.
            _enumerationDirections = _diff.IndivFunc(_diff, (a, b) =>  a != 0 ? a / Math.Abs(b) : 0); //scales each vector component down to -1, 0, or 1;
            Reset();
        }

        public SpatialEnumerator(Array array, int[] startIndex, int[] endIndex) : this(array, startIndex, endIndex, NDimArray.GetStandardEnumerationPriorities(array.Rank)) { }

        public SpatialEnumerator(Array array) : this(array, array.GetLowerBoundaries(), array.GetUpperBoundaries()) { }

        public void Reset()
        {
            firstEvaluated = false;
            _currentIndex = (int[])_startIndices.Clone(); //sets current index to the startPoint
        }

        public bool MoveNext()
        {
            if (firstEvaluated)
            {
                if (!_currentIndex.SequenceEqual(_endIndices))
                {
                    Increment(0);
                    return true;
                }
                else return false;
            }
            else
            {
                firstEvaluated = true;
                return true;
            }
        }

        private void Increment(int priorityIndex)
        {
            int dimToIncrement = _dimPriorityList[priorityIndex];
            int incrementationAmount = _enumerationDirections[dimToIncrement];

            if (ShouldMove())
            {
                _currentIndex[dimToIncrement] = _startIndices[dimToIncrement];
                Increment(priorityIndex + 1);
            } else
            {
                _currentIndex[dimToIncrement] += incrementationAmount;
            }

            bool ShouldMove()
            {
                return _currentIndex[dimToIncrement] == _endIndices[dimToIncrement];
            }
        }
        
        public void Dispose() { }

        public static int[] GetStandardPriorityList(int rank)
        {
            int[] array = new int[rank];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rank - i - 1;
            }

            return array;
        }

        /// <param name="func">function that takes in a dimension index</param>
        /// <returns></returns>
        internal static int[] ForEachDim(Array a, Func<int, int> func)
        {
            int[] retArray = new int[a.Rank];

            for (int i = 0; i < a.Rank; i++)
            {
                retArray[i] = func(i);
            }

            return retArray;
        }
    }

    internal partial class SpatialEnumerator<T>
    {
        void ValidateConstructor(Array array, int[] startIndices, int[] endIndices, int[] dimPriorityList)
        {
            //null checks
            if (startIndices == null)
                throw new ArgumentNullException("startIndices", "startIndices is null");
            if (endIndices == null)
                throw new ArgumentNullException("endIndices", "endIndices is null");
            if (dimPriorityList == null)
                throw new ArgumentNullException("dimPriorityList", "dimPriorityList is null");

            //length checks
            if (array.Rank != startIndices.Length)
                throw new ArgumentOutOfRangeException("startIndices", "startIndices length must be the same as the rank of the array");
            if (array.Rank != endIndices.Length)
                throw new ArgumentOutOfRangeException("endIndices", "endIndices length must be the same as the rank of the array");
            if (array.Rank != endIndices.Length)
                throw new ArgumentOutOfRangeException("dimPriorityList", "dimPriorityList length must be the same as the rank of the array");

            //boundary tests
            if (!dimPriorityList.ElementsUnique())
                throw new ArgumentException("dimPriorityList", "dimPriorityList elements must be unique");
            if (!Array.TrueForAll(dimPriorityList, x => x >= 0 && x < dimPriorityList.Length))
                throw new ArgumentException("dimPriorityList", "dimPriorityList elements must be a sequence with no gaps between 0 and length - 1");
            if (!ValidIndex(startIndices))
                throw new ArgumentOutOfRangeException("startIndices", "startIndices are not within the bounds of the array");
            if (!ValidIndex(endIndices))
                throw new ArgumentOutOfRangeException("endIndices", "endIndices are not within the bounds of the array");
        }

        bool ValidIndex(int[] index)
        {
            for (int i = 0; i < index.Length; i++)
            {
                int item = index[i];
                if (item < array.GetLowerBound(i) || item > array.GetUpperBound(i))
                    return false;
            }
            return true;
        }
    }
}
