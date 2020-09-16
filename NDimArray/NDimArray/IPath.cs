namespace NDimArray
{
    public interface IPath
    {
        int[] Start { get; }
        int[] End { get; }
        IEnumerationPriorities DimEnumerationPriorities { get; }

        /// <summary>
        /// The defined length of every component.
        /// </summary>
        int DimensionCount { get; }
    }
}