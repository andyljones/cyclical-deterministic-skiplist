using System;
using System.Collections.Generic;

namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head;

        public readonly int MinimumGapSize = 2;
        public readonly int MaximumGapSize = 4;

        public readonly Func<T, T, T, bool> Compare;


        public Skiplist()
        {
            Compare = CreateCyclicComparer(Comparer<T>.Default.Compare);
        }

        public Skiplist(int minimumGapSize) : this()
        {
            MinimumGapSize = minimumGapSize;
            MaximumGapSize = 2*minimumGapSize;
        }


        private static Func<T, T, T, bool> CreateCyclicComparer(Func<T, T, int> linearCompare)
        {
            return new LinearCompareToCyclicContainsAdapter<T>(linearCompare).CyclicContains;
        }

        public override string ToString()
        {
            return SkiplistStringFormatter.StringOf(this);
        }

        public INode<T> CreateNode(T key)
        {
            return new Node<T> {Key = key};
        }
    }
}
