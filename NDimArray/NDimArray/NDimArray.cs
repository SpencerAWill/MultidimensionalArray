using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public class NDimArray<T> : IEnumerable<T>, IEnumerable<Tuple<int[], T>>
    {
        private readonly Array array;

        /// <summary>
        /// The amount of dimensions of this array.
        /// </summary>
        public int Rank => array.Rank;

        /// <summary>
        /// The total number of elements in all dimensions.
        /// </summary>
        public int Length => array.Length;

        /// <summary>
        /// Gets an object that synchronizes access to the array.
        /// </summary>
        public object SyncRoot => array.SyncRoot;

        public bool IsFixedSize => true;
        public virtual bool IsSynchronized => false;
        public bool IsReadOnly => false;

        public T this[params int[] index]
        {
            get => GetValue(index);
            set => SetValue(value, index);
        }

        #region Constructors
        public NDimArray(int[] lengths, int[] lowerBounds)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths", "dimensions array is null");
            if (lowerBounds == null)
                throw new ArgumentNullException("lowerBounds", "lowerBounds array is null");
            if (lengths.Length < 1)
                throw new ArgumentException("lengths", "lengths must have at least 1 element");
            if (lowerBounds.Length < 1)
                throw new ArgumentException("lowerBounds", "lowerBounds must have at least 1 element");

            if (lengths.Length != lowerBounds.Length)
                throw new ArgumentException("lowerBounds", "length of lowerBounds must be equal to the length of lengths");

            if (ValidDimensions(lengths))
                array = Array.CreateInstance(typeof(T), lengths, lowerBounds);
            else
                throw new ArgumentOutOfRangeException("lengths", "Every length must be > 0.");
        }

        public NDimArray(params int[] lengths) : 
            this(
                lengths, 
                lengths != null ? 
                    (lengths.Length > 0 ? 
                        (int[])Array.CreateInstance(typeof(int), lengths.Length) 
                        : throw new ArgumentException("lengths", "lengths myst have at least 1 element")) 
                    : throw new ArgumentNullException("lengths","lengths was null")
                ) { }

        public NDimArray(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array","array is null");

            this.array = array;
        }
        #endregion

        #region Getters/Setters
        public virtual T GetValue(params int[] index)
        {
            if (ValidIndices(index))
                return (T)array.GetValue(index);
            else
                return default;
        }

        public virtual void SetValue(T value, params int[] index)
        {
            if (ValidIndices(index))
                array.SetValue(value, index);
        }

        protected virtual bool ValidIndices(int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices", "indices was null");

            if (indices.Length != Rank)
                throw new ArgumentOutOfRangeException("indices", "index must match the rank of the NDimArray");

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] < array.GetLowerBound(i) || indices[i] > array.GetUpperBound(i))
                    return false;
                else
                    continue;
            }
            return true;
        }

        protected virtual bool ValidDimensions(int[] dims)
        {
            return Array.TrueForAll(dims, x => x > 0);
        }
        #endregion

        #region Enumeration
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => new SpatialEnumerator<T>(array);

        IEnumerator<Tuple<int[], T>> IEnumerable<Tuple<int[], T>>.GetEnumerator() => (IEnumerator<Tuple<int[], T>>)(new SpatialEnumerator<T>(array));

        public void Enumerate(Action<T> action)
        {
            foreach (var item in this)
            {
                action(item);
            }
        }
        public void Enumerate(Action<int[], T> action)
        {
            foreach (var item in (IEnumerable<Tuple<int[], T>>)this)
            {
                action(item.Item1, item.Item2);
            }
        }

        public void Enumerate(int[] start, int[] end, int[] dimPriorities, Action<int[], T> action)
        {
            using (IEnumerator<Tuple<int[], T>> enumer = new SpatialEnumerator<T>(array, start, end, dimPriorities))
            {
                while (enumer.MoveNext())
                {
                    var tup = enumer.Current;
                    action(tup.Item1, tup.Item2);
                }
            }
        }
        public void Enumerate(int[] start, int[] end, int[] dimPriorities, Action<T> action) =>
            Enumerate(start, end, dimPriorities, (index, item) => action(item));

        public void Enumerate(int[] start, int[] end, Action<int[], T> action) => 
            Enumerate(start, end, SpatialEnumerator<T>.GetStandardPriorityList(Rank), action);
        public void Enumerate(int[] start, int[] end, Action<T> action) =>
            Enumerate(start, end, (index, item) => action(item));
        #endregion

        #region Methods
        public int GetLowerBound(int dimension)
        {
            return array.GetLowerBound(dimension);
        }

        public int GetUpperBound(int dimension)
        {
            return array.GetUpperBound(dimension);
        }

        public int[] GetStandardEnumerationPriorities()
        {
            return SpatialEnumerator<T>.GetStandardPriorityList(Rank);
        }
        #endregion

        #region Statics

        public static void Fill(NDimArray<T> array, int[] start, int[] end, int[] dimPriorities, Func<int[], T, T> fillRule)
        {
            //THERE ARE WAY TOO MANY CONDITIONS HERE
            //perhaps move parameters: start, end, dimPriorities to a separate class/struct which does the checks (SRP)

            // null checks
            if (array == null)
                throw new ArgumentNullException("array", "array is null");
            if (start == null)
                throw new ArgumentNullException("start", "start is null");
            if (end == null)
                throw new ArgumentNullException("end", "end is null");
            if (array == null)
                throw new ArgumentNullException("dimPriorities", "dimPriorities is null");

            //no item checks
            if (start.Length < 1)
                throw new ArgumentException("start", "start must have at least one element");
            if (end.Length < 1)
                throw new ArgumentException("end", "end must have at least one element");
            if (dimPriorities.Length < 1)
                throw new ArgumentException("dimPriorities", "dimPriorities must have at least one element");

            //length checks
            if (start.Length != array.Rank)
                throw new ArgumentException("start", "start must have the same number of indices as the rank of the array");
            if (end.Length != array.Rank)
                throw new ArgumentException("end", "end must have the same number of indices as the rank of the array");
            if (dimPriorities.Length != array.Rank)
                throw new ArgumentException("dimPriorities", "dimPriorities must have the same number of indices as the rank of the array");

            if (!array.ValidIndices(start))
                throw new IndexOutOfRangeException("1 or more dimension indices in start was out of the range of the array");
            if (!array.ValidIndices(end))
                throw new IndexOutOfRangeException("1 or more dimension indices in end was out of the range of the array");
            if (!dimPriorities.SequenceEqual(dimPriorities.Distinct()))
                throw new ArgumentException("dimPriorities", "dimPriorities must be distinct (every element must be unique)");
            if (!Array.TrueForAll(dimPriorities, x => x >= 0 && x <= array.Rank - 1))
                throw new ArgumentOutOfRangeException("dimPriorities", "one or more elements of dimPriorities was an invalid dimension");

            array.Enumerate(start, end, dimPriorities, (index, item) => array[index] = fillRule(index, item));
        }

        public static void Fill(NDimArray<T> array, int[] start, int[] end, int[] dimPriorities, T value) =>
            Fill(array, start, end, dimPriorities, (index, item) => value);

        public static void Fill(NDimArray<T> array, int[] start, int[] end, Func<int[], T, T> fillRule) =>
            Fill(array, start, end, array.GetStandardEnumerationPriorities(), fillRule);

        public static void Fill(NDimArray<T> array, int[] start, int[] end, T value) =>
            Fill(array, start, end, (index, item) => value);

        public static void Fill(NDimArray<T> array, Func<int[], T, T> fillRule) =>
            Fill(
                array,
                SpatialEnumerator<T>.LowerBoundaries(array.array),
                SpatialEnumerator<T>.UpperBoundaries(array.array),
                array.GetStandardEnumerationPriorities(),
                fillRule);

        public static void Fill(NDimArray<T> array, T value) =>
            Fill(array, (index, item) => value);

        public static NDimArray<T> Sub(NDimArray<T> array, int[] start, int[] end, bool preserveIndices = false) =>
            throw new NotImplementedException();

        #endregion
    }
}
