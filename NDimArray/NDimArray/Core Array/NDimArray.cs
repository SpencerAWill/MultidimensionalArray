using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{

    public partial class NDimArray<T> : 
        INDimArray<T>, 
        IReadOnlyNDimArray<T>, 
        IEnumerable<T>, 
        IEnumerable<Tuple<int[], T>>
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
        public bool IsReadOnly => true;
        bool INDimArray<T>.IsReadOnly => false;

        public T this[params int[] index]
        {
            get => GetValue(index);
            set => SetValue(value, index);
        }

        #region Constructors
        public NDimArray(int[] lengths, int[] lowerBounds)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths", "lengths is null");
            if (lowerBounds == null)
                throw new ArgumentNullException("lowerBounds", "lowerBounds is null");

            if (lengths.Length == 0)
                throw new ArgumentException("lengths", "lengths must have at least 1 element");

            if (!ValidDimensions(lengths))
                throw new ArgumentOutOfRangeException("lengths", "all lengths must be greater than 0.");

            if (lengths.Length != lowerBounds.Length)
                throw new ArgumentException("lowerBounds", "lowerBounds and lengths must be the same lengths");

            array = Array.CreateInstance(typeof(T), lengths, lowerBounds);
        }

        public NDimArray(params int[] lengths)
        {
            if (lengths == null)
                throw new ArgumentNullException("lengths", "lengths is null");

            if (lengths.Length == 0)
                throw new ArgumentException("lengths", "lengths must have at least 1 element");

            var origin = new int[lengths.Length];

            if (!ValidDimensions(lengths))
                throw new ArgumentOutOfRangeException("lengths", "all lengths must be greater than 0.");

            array = Array.CreateInstance(typeof(T), lengths, origin);
        }

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
            if (ValidIndex(index))
                return (T)array.GetValue(index);
            else
                throw new IndexOutOfRangeException("one or more indices was out of range");
        }

        public virtual void SetValue(T value, params int[] index)
        {
            if (ValidIndex(index))
                array.SetValue(value, index);
        }

        public virtual bool ValidIndex(params int[] index)
        {
            _ = index ?? throw new ArgumentNullException(nameof(index));

            if (index.Length != Rank)
                throw new ArgumentOutOfRangeException(nameof(index), "index must equal the rank of the NDimArray");

            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] < array.GetLowerBound(i) || index[i] > array.GetUpperBound(i))
                    return false;
            }
            return true;
        }
        #endregion

        protected virtual bool ValidDimensions(int[] dims) => dims.All(x => x > 0);

        #region Enumeration
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() => new P2PEnumerator<T>(array);
        IEnumerator<Tuple<int[], T>> IEnumerable<Tuple<int[], T>>.GetEnumerator() => (IEnumerator<Tuple<int[], T>>)(new P2PEnumerator<T>(array));

        public void Enumerate(Action<T> action)
        {
            Enumerate((array, item) => action(item));
        }
        public void Enumerate(Action<int[], T> action)
        {
            var path = new IndexPath(GetLowerBoundaries(), GetUpperBoundaries());
            Enumerate(path, (array, item) => action(array, item));
        }
        public void Enumerate(IPath path, Action<T> action)
        {
            Enumerate(path, (index, item) => action(item));
        }

        public void Enumerate(IPath path, Action<int[], T> action)
        {
            using (IEnumerator<Tuple<int[], T>> enumer = new P2PEnumerator<T>(array, path))
            {
                while (enumer.MoveNext())
                {
                    var tup = enumer.Current;
                    action(tup.Item1, tup.Item2);
                }
            }
        }
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
