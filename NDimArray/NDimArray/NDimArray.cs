using System;
using System.Collections;
using System.Collections.Generic;
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

            if (ValidDimensions(lengths))
                array = Array.CreateInstance(typeof(T), lengths, lowerBounds);
            else
                throw new ArgumentOutOfRangeException("lengths", "Every dimension must be >= 0.");
        }
        public NDimArray(params int[] dimensions) : this(dimensions, (int[])Array.CreateInstance(typeof(int), dimensions.Length)) { }
        public NDimArray(Array array)
        {
            if (array != null)
                this.array = array;
        }
        public NDimArray(T[] array) : this((Array)array) { }
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
                if (indices[i] < array.GetLowerBound(i))
                    return false;
                else
                    continue;
            }
            return true;
        }

        protected virtual bool ValidDimensions(int[] dims)
        {
            return Array.TrueForAll(dims, x => x >= 0);
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

        public void EnumerateCustom(int[] start, int[] end, int[] dimPriorities, Action<int[], T> action)
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

        public void EnumerateCustom(int[] start, int[] end, int[] dimPriorities, Action<T> action)
        {
            EnumerateCustom(start, end, dimPriorities, (index, item) => action(item));
        }

        public void EnumerateFrom(int[] start, int[] end, Action<int[], T> action) =>
            EnumerateCustom(start, end, SpatialEnumerator<T>.GetStandardPriorityList(array.Rank), action);

        public void EnumerateFrom(int[] start, int[] end, Action<T> action) =>
            EnumerateFrom(start, end, (index, item) => action(item));
        public void EnumerateFrom(int[] start, Action<int[], T> action) =>
            EnumerateFrom(start, SpatialEnumerator<T>.UpperBoundaries(array), (index, item) => action(index, item));

        public void EnumerateFrom(int[] start, Action<T> action) =>
            EnumerateFrom(start, (index, item) => action(item));

        public void EnumerateTo(int[] end, Action<int[], T> action) =>
            EnumerateCustom(SpatialEnumerator<T>.LowerBoundaries(array), end, SpatialEnumerator<T>.GetStandardPriorityList(array.Rank), action);

        public void EnumerateTo(int[] end, Action<T> action) =>
            EnumerateTo(end, (index, item) => action(item));
        #endregion
    }
}
