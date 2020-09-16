using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public class IndexPath : IPath
    {
        private int[] _start;
        private int[] _end;
        private IEnumerationPriorities _priorities;

        public int[] Start { get => _start; }
        public int[] End { get => _end; }
        public IEnumerationPriorities DimEnumerationPriorities { get => _priorities; }

        public int DimensionCount => DimEnumerationPriorities.Priorities.Count;

        public IndexPath(int[] start, int[] end, IEnumerationPriorities dimEnumerationPriorities)
        {
            Verify(start, end, dimEnumerationPriorities);

            _start = start;
            _end = end;
            _priorities = dimEnumerationPriorities;
        }

        public IndexPath(int[] start, int[] end)
        {
            Verify(start, end);

            var defPrior = EnumerationPriorities.CreateStandard(start.Length);

            _start = start;
            _end = end;
            _priorities = defPrior;
        }

        private void Verify(int[] start, int[] end, IEnumerationPriorities dimEnumerationPriorities)
        {
            Verify(start, end);

            if (dimEnumerationPriorities == null)
                throw new ArgumentNullException("dimEnumerationPriorities", "dimEnumerationPriorities is null");

            if (dimEnumerationPriorities.Priorities.Count == 0)
                throw new ArgumentException("dimEnumerationPriorities", "dimEnumerationPriorities must have at least 1 element");

            if (start.Length != dimEnumerationPriorities.Priorities.Count)
                throw new ArgumentException("dimEnumerationPriorities", "dimEnumerationPriorities must have length equal to that of start indices");
        }

        private void Verify(int[] start, int[] end)
        {
            if (start == null)
                throw new ArgumentNullException("start", "start is null");
            if (end == null)
                throw new ArgumentNullException("end", "end is null");

            if (start.Length != end.Length)
                throw new ArgumentException("end", $"Cannot path {start.Length}D start index with {end.Length}D end index.");

            if (start.SequenceEqual(end))
                throw new ArgumentException("end", "start and end are the same point");
        }
    }
}
