using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public class Skiplist<T> : IEnumerable<INode<T>> 
        where T : IComparable<T>
    {
        public INode<T> ReferenceNode { get; set; }

        public IEnumerator<INode<T>> GetEnumerator()
        {
            INode<T> currentNode = ReferenceNode;

            while (currentNode != null)
            {
                yield return currentNode;
                currentNode = currentNode.Right;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(INode<T> node)
        {
            ReferenceNode = node;
        }
    }
}
