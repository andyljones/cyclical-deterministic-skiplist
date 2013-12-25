using System;
using System.Collections;
using System.Collections.Generic;

namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head;

        public readonly int MinimumGapSize = 2;
        public readonly int MaximumGapSize = 4;

        public readonly Func<T, T, T, bool> InOrder;
        public readonly Func<T, INode<T>> NodeFactory = SkiplistUtilities.CreateNode; 

        public Skiplist(int minimumGapSize = 2, Func<T, T, T, bool> inOrder = null)
        {
            InOrder = inOrder ?? new CompareToCyclicOrdererAdapter<T>(Comparer<T>.Default.Compare).InOrder;
            MinimumGapSize = minimumGapSize;
            MaximumGapSize = 2 * minimumGapSize;
        }

        public override string ToString()
        {
            return SkiplistStringFormatter.StringOf(this);
        }
    }
}
