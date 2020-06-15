namespace NDimArray
{
    public interface IPath
    {
        INIndex Start { get; }
        INIndex End { get; }
        IEnumerationPriorities DimEnumerationPriorities { get; }

        /// <summary>
        /// The defined length of every component.
        /// </summary>
        int DimensionCount { get; }
    }
}