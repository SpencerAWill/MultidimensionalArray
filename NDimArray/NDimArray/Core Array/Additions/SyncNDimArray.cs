namespace NDimArray
{
    public partial class NDimArray<T>
    {
        public static NDimArray<T> Synchronize(NDimArray<T> array)
        {
            return new SyncNDimArray<T>(array);
        }
    }

    internal sealed class SyncNDimArray<T> : NDimArray<T>
    {
        internal SyncNDimArray(NDimArray<T> array) : base(array.GetLengths(), array.GetLowerBoundaries())
        {
            array.Enumerate((index, item) => this[index] = item);
        }

        public override bool IsSynchronized => true;

        public override T GetValue(params int[] index)
        {
            lock (SyncRoot)
            {
                return base.GetValue(index);
            }
        }

        public override void SetValue(T value, params int[] index)
        {
            lock (SyncRoot)
            {
                base.SetValue(value, index);
            }
        }
    }
}
