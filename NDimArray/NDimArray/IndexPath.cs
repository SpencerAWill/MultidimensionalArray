using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public class IndexPath : IPath
    {
        private INIndex _start;
        private INIndex _end;
        private IEnumerationPriorities _priorities;

        public INIndex Start { get => _start; }
        public INIndex End { get => _end; }
        public IEnumerationPriorities DimEnumerationPriorities { get => _priorities; }

        public int DimensionCount => DimEnumerationPriorities.Priorities.Count;

        public IndexPath(INIndex start, INIndex end, IEnumerationPriorities dimEnumerationPriorities)
        {
            Verify(start, end, dimEnumerationPriorities);

            _start = start;
            _end = end;
            _priorities = dimEnumerationPriorities;
        }

        public IndexPath(INIndex start, INIndex end)
        {
            Verify(start, end);

            var defPrior = EnumerationPriorities.CreateStandard(start.Indices.Count);

            _start = start;
            _end = end;
            _priorities = defPrior;
        }

        private void Verify(INIndex start, INIndex end, IEnumerationPriorities dimEnumerationPriorities)
        {
            Verify(start, end);

            if (dimEnumerationPriorities == null)
                throw new ArgumentNullException("dimEnumerationPriorities", "dimEnumerationPriorities is null");

            if (dimEnumerationPriorities.Priorities.Count == 0)
                throw new ArgumentException("dimEnumerationPriorities", "dimEnumerationPriorities must have at least 1 element");

            if (start.Indices.Count != dimEnumerationPriorities.Priorities.Count)
                throw new ArgumentException("dimEnumerationPriorities", "dimEnumerationPriorities must have length equal to that of start indices");
        }

        private void Verify(INIndex start, INIndex end)
        {
            if (start == null)
                throw new ArgumentNullException("start", "start is null");
            if (end == null)
                throw new ArgumentNullException("end", "end is null");

            if (start.Indices.Count != end.Indices.Count)
                throw new ArgumentException("end", $"Cannot path {start.Indices.Count}D start index with {end.Indices.Count}D end index.");

            if (start.Indices.SequenceEqual(end.Indices))
                throw new ArgumentException("end", "start and end are the same point");
        }
    }
}
