using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDimArray
{
    public class EnumerationPriorities : IEnumerationPriorities
    {
        private int[] _priorities;

        public IReadOnlyList<int> Priorities { get => _priorities; }

        public int this[int index] => Priorities[index];

        public EnumerationPriorities(int[] priorities)
        {
            Verify(priorities);

            _priorities = priorities;
        }

        private void Verify(int[] priorities)
        {
            if (priorities == null)
                throw new ArgumentNullException("priorities", "priorities is null");
            if (priorities.Length == 0)
                throw new ArgumentException("priorities", "priorities must have at least 1 element");
            if (!priorities.SequenceEqual(priorities.Distinct()))
                throw new ArgumentException("priorities", "elements of priorities must be unique");
            if (!Array.TrueForAll(priorities, x => x >= 0 && x < priorities.Length))
                throw new ArgumentOutOfRangeException("priorities", "one or more elements in priorities is not a valid dimension");
        }

        public static EnumerationPriorities CreateStandard(int rank)
        {
            int[] ret = new int[rank];
            for (int i = 0; i < rank; i++)
            {
                ret[i] = rank - i - 1;
            }
            return new EnumerationPriorities(ret);
        }
    }

    public interface IEnumerationPriorities
    {
        IReadOnlyList<int> Priorities { get; }
        int this[int index] { get; }
    }
}
