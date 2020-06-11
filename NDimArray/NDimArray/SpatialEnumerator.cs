using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    internal sealed class SpatialEnumerator<T> : IEnumerator<T>, IEnumerator<Tuple<int[], T>>
    {
        private Array _array;
        private int[] _diff;
        private int[] _enumerationDirections;
        private EnumerationPath _path;
        private int[] _currentIndex;
        private bool _firstEvaluated;

        private Array Array { get => _array; }
        public IReadOnlyList<int> Difference { get => _diff; }
        public IReadOnlyList<int> EnumerationDirections { get => _enumerationDirections; }
        public EnumerationPath Path { get => _path; }
        private int[] CurrentIndex { get => _currentIndex; }
        private bool FirstEvaluated { get => _firstEvaluated; set => _firstEvaluated = value; }

        public T Current => 
            (T)_array.GetValue(_currentIndex);

        Tuple<int[], T> IEnumerator<Tuple<int[], T>>.Current => 
            new Tuple<int[], T>(_currentIndex, Current);

        object IEnumerator.Current => Current;

        public SpatialEnumerator(Array array, EnumerationPath path)
        {
            if (array == null)
                throw new ArgumentNullException("array", "array is null");
            if (path == null)
                throw new ArgumentNullException("path", "path is null");

            if (array.Rank != path.Start.Length || array.Rank != path.End.Length || array.Rank != path.DimEnumerationPriorities.Length)
                throw new ArgumentException("path", "path properties have more elements than the rank of the array");

            _array = array;
            _path = path;

            _diff = Path.End.SubtractEach(Path.Start);
            _enumerationDirections = _diff.IndivFunc(_diff, (a, b) => a != 0 ? a / Math.Abs(b) : 0);
            Reset();
        }

        public SpatialEnumerator(Array array) : this(
            array, 
            new EnumerationPath(
                array.GetLowerBoundaries(), 
                array.GetUpperBoundaries())) { }

        public void Reset()
        {
            FirstEvaluated = false;
            _currentIndex = (int[])Path.Start.Clone(); //sets current index to the startPoint
        }

        public bool MoveNext()
        {
            if (FirstEvaluated)
            {
                if (!CurrentIndex.SequenceEqual(Path.End))
                {
                    Increment(0);
                    return true;
                }
                else return false;
            }
            else
            {
                FirstEvaluated = true;
                return true;
            }
        }

        private void Increment(int priorityIndex)
        {
            int dimToIncrement = Path.DimEnumerationPriorities[priorityIndex];
            int incrementationAmount = EnumerationDirections[dimToIncrement];

            if (ShouldMove()) //when the end of the current dimension is reached
            {
                CurrentIndex[dimToIncrement] = Path.Start[dimToIncrement]; //reset current index back
                Increment(priorityIndex + 1); //increment next dimension
            } else
            {
                CurrentIndex[dimToIncrement] += incrementationAmount;
            }

            bool ShouldMove()
            {
                return CurrentIndex[dimToIncrement] == Path.End[dimToIncrement];
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
}
