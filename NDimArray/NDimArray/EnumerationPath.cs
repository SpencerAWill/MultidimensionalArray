using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public struct EnumerationPath
    {
        private int[] _start;
        private int[] _end;
        private int[] _priorities;

        public int[] Start { get => _start; }
        public int[] End { get => _end; }
        public int[] DimEnumerationPriorities { get => _priorities; }


        public EnumerationPath(int[] start, int[] end, int[] dimPriorities)
        {
            Verify(start, end, dimPriorities);

            _start = start;
            _end = end;
            _priorities = dimPriorities;
        }

        public EnumerationPath(int[] start, int[] end) : this(
            start,
            end,
            start != null ? 
                (start.Length > 0 ? NDimArray.GetStandardEnumerationPriorities(start.Length) : throw new ArgumentOutOfRangeException("start", "start must have at least 1 element")) 
            : throw new ArgumentNullException("start", "start is null")) { }

        private static void Verify(int[] start, int[] end, int[] dimPriorities)
        {
            //null checks
            if (start == null)
                throw new ArgumentNullException("start", "start is null");
            if (end == null)
                throw new ArgumentNullException("end", "end is null");
            if (dimPriorities == null)
                throw new ArgumentNullException("dimPriorities", "dimPriorities is null");

            //empty checks
            if (start.Length < 1)
                throw new ArgumentException("start", "start must have at least 1 element");
            if (end.Length < 1)
                throw new ArgumentException("end", "end must have at least 1 element");
            if (dimPriorities.Length < 1)
                throw new ArgumentException("dimPriorities", "dimPriorities must have at least 1 element");

            //length checks
            if (start.Length != end.Length)
                throw new ArgumentException("end", "end must have the same number of elements as start");
            if (start.Length != dimPriorities.Length)
                throw new ArgumentException("dimPriorities", "dimPriorities must have the same number of elements as start");

            //equivalence check
            if (start.SequenceEqual(end))
                throw new ArgumentException("end", "start and end must not be the same point");

            //dimPriorities check
            if (!dimPriorities.SequenceEqual(dimPriorities.Distinct()))
                throw new ArgumentException("dimPriorities", "every element in dimPriorities must be unique.");
            if (!Array.TrueForAll(dimPriorities, x => x >= 0 && x < dimPriorities.Length))
                throw new ArgumentOutOfRangeException("dimPriorities", "one or more elements of dimPriorities was an invalid dimension");
        }
    }
}
