using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{

    public partial class NDimArray<T> : INDimArray<T>, IReadOnlyNDimArray<T>, IEnumerable<T>, IEnumerable<Tuple<INIndex, T>>
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
        bool IReadOnlyNDimArray<T>.IsReadOnly => true;

        public T this[params int[] index]
        {
            get => GetValue(index);
            set => SetValue(value, index);
        }

        public T this[INIndex index]
        {
            get => GetValue(index);
            set => SetValue(value, index);
        }

        #region Constructors
        public NDimArray(int[] lengths, INIndex lowerBounds)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths", "lengths is null");
            if (lowerBounds == null)
                throw new ArgumentNullException("lowerBounds", "lowerBounds is null");

            if (lengths.Length == 0)
                throw new ArgumentException("lengths", "lengths must have at least 1 element");

            if (!ValidDimensions(lengths))
                throw new ArgumentOutOfRangeException("lengths", "all lengths must be greater than 0.");

            if (lengths.Length != lowerBounds.Indices.Count)
                throw new ArgumentException("lowerBounds", "lowerBounds and lengths must be the same lengths");

            array = Array.CreateInstance(typeof(T), lengths, lowerBounds.Indices.ToArray());
        }

        public NDimArray(int[] lengths, int[] lowerBounds) : this(lengths, new NIndex(lowerBounds)) { }

        public NDimArray(params int[] lengths)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths", "lengths is null");

            if (lengths.Length == 0)
                throw new ArgumentException("lengths", "lengths must have at least 1 element");

            INIndex origin = NIndex.Origin(lengths.Length);

            if (!ValidDimensions(lengths))
                throw new ArgumentOutOfRangeException("lengths", "all lengths must be greater than 0.");

            array = Array.CreateInstance(typeof(T), lengths, origin.Indices.ToArray());
        }

        public NDimArray(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array","array is null");

            this.array = array;
        }
        #endregion

        #region Getters/Setters

        #region int[]
        public virtual T GetValue(params int[] index)
        {
            if (ValidIndices(index))
                return (T)array.GetValue(index);
            else
                throw new IndexOutOfRangeException("one or more indices was out of range");
        }

        public virtual void SetValue(T value, params int[] index)
        {
            if (ValidIndices(index))
                array.SetValue(value, index);
        }

        protected virtual bool ValidIndices(int[] index)
        {
            if (index == null)
                throw new ArgumentNullException("indices", "indices was null");

            if (index.Length != Rank)
                throw new ArgumentOutOfRangeException("indices", "index must match the rank of the NDimArray");

            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] < array.GetLowerBound(i) || index[i] > array.GetUpperBound(i))
                    return false;
                else
                    continue;
            }
            return true;
        }
        #endregion

        #region IReadOnlyNIndex
        public virtual T GetValue(INIndex index)
        {
            if (ValidIndex(index))
                return GetValue(index.Indices.ToArray());
            else
                throw new IndexOutOfRangeException("one or more indices was out of range");
        }

        public virtual void SetValue(T value, INIndex index)
        {
            if (ValidIndex(index))
                SetValue(value, index.Indices.ToArray());

        }

        protected virtual bool ValidIndex(INIndex index)
        {
            if (index == null)
                throw new ArgumentNullException("index", "index is null");

            for (int i = 0; i < index.Indices.Count; i++)
            {
                if (index[i] < array.GetLowerBound(i) || index[i] > array.GetUpperBound(i))
                    return false;
                else
                    continue;
            }
            return true;
        }
        #endregion

        protected virtual bool ValidDimensions(int[] dims)
        {
            return Array.TrueForAll(dims, x => x > 0);
        }
        #endregion

        #region Enumeration
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => new P2PEnumerator<T>(array);

        IEnumerator<Tuple<INIndex, T>> IEnumerable<Tuple<INIndex, T>>.GetEnumerator() => (IEnumerator<Tuple<INIndex, T>>)(new P2PEnumerator<T>(array));

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

        public void Enumerate(IPath path, Action<INIndex, T> action)
        {
            using (IEnumerator<Tuple<INIndex, T>> enumer = new P2PEnumerator<T>(array, path))
            {
                while (enumer.MoveNext())
                {
                    var tup = enumer.Current;
                    action(tup.Item1, tup.Item2);
                }
            }
        }

        public void Enumerate(IPath path, Action<T> action) =>
            Enumerate(path, (index, item) => action(item));
        #endregion

        #region Methods
        public int[] GetLowerBoundaries() =>
            array.GetLowerBoundaries();
        public int[] GetUpperBoundaries() =>
            array.GetUpperBoundaries();

        public int GetUpperBound(int dimension) => 
            array.GetUpperBound(dimension);
        public int GetLowerBound(int dimension) => 
            array.GetLowerBound(dimension);

        public int GetLength(int dimension) =>
            array.GetLength(dimension);

        public int[] GetLengths() =>
            array.DimEnumerator(x => array.GetLength(x));

        public EnumerationPriorities GetStandardEnumerationPriorities() =>
            EnumerationPriorities.CreateStandard(Rank);
        #endregion
    }
}
