using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public class Skiplist<T> : ICollection<T>
    {
        public INode<T> Head;

        public readonly int MinimumGapSize;
        public readonly int MaximumGapSize;

        public readonly Func<T, T, T, bool> InOrder;
        public readonly Func<T, INode<T>> NodeFactory = SkiplistUtilities.CreateNode; 

        public Skiplist(int minimumGapSize = 2, Func<T, T, T, bool> inOrder = null)
        {
            InOrder = inOrder ?? new CompareToCyclicOrdererAdapter<T>(Comparer<T>.Default.Compare).InOrder;
            MinimumGapSize = minimumGapSize;
            MaximumGapSize = 2 * minimumGapSize;
        }

        #region ICollection methods
        public int Count
        {
            get
            {
                return Head == null ? 0 : Head.Bottom().LengthOfList();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Contains(T item)
        {
            return this.Find(item);
        }

        public void Add(T item)
        {
            this.Insert(item);
        }

        public bool Remove(T item)
        {
            return this.Delete(item);
        }

        public void Clear()
        {
            Head = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Head != null)
            {
                var bottomNodeEnumerator = Head.Bottom().EnumerateRight().GetEnumerator();
                while (bottomNodeEnumerator.MoveNext())
                {
                    yield return bottomNodeEnumerator.Current.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (Head != null)
            {
                Head.Bottom()
                    .EnumerateRight()
                    .Select(node => node.Key)
                    .ToArray()
                    .CopyTo(array, arrayIndex);
            }
        }
        #endregion

        public INode<T> NodeContaining(T key)
        {
            return this.FetchGap(key);
        }

        public override string ToString()
        {
            return SkiplistStringFormatter.StringOf(this);
        }

    }
}
