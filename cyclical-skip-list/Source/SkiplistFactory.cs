using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipListTests;

namespace CyclicalSkipList
{
    public static class SkiplistFactory
    {
        public static INode<T> CreateFrom<T>(List<T> keys)
        {
            if (keys.Count == 0)
            {
                return null;
            }

            var sortedKeys = keys.OrderBy(key => key).ToList();

            var head = new Node<T>(sortedKeys.First());
            INode<T> currentNode = head;
            foreach (var key in sortedKeys.Skip(1))
            {
                currentNode.InsertToRight(new Node<T>(key));
                currentNode = currentNode.Right;
            }
            currentNode.Right = head;


            return head;
        }
    }
}
