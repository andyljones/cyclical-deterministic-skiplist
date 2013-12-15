using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head { get; private set; }

        private readonly Func<T, T, int> _compare;
        private readonly Func<T, T, bool> _equals; 

        public Skiplist()
        {
            _compare = Comparer<T>.Default.Compare;
            _equals = EqualityComparer<T>.Default.Equals;
        }

        public Skiplist(INode<T> head) : this()
        {
            Head = head;
        }

        public bool Contains(T key)
        {
            var ends = SkiplistUtilities.EnumerateLevels(Head).ToList();

            var currentNode = Head;
            while (true)
            {
                while (_compare(key, currentNode.Key) > 0 &&
                       !ends.Contains(currentNode.Right))
                {
                    currentNode = currentNode.Right;
                }

                if (currentNode.Down == null)
                {
                    return (_equals(key, currentNode.Key));
                }

                currentNode = currentNode.Down;
            }
        }
    }
}
