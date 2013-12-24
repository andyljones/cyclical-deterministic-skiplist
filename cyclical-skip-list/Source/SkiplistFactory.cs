using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistFactory
    {
        public static int MinimumGapSize = 2;
        public static int MaximumGapSize = MinimumGapSize*2;

        public static Skiplist<T> CreateFrom<T>(IEnumerable<T> keys)
        {
            var keyList = keys.ToList();
            var skiplist = new Skiplist<T>(MinimumGapSize);

            if (keyList.Any())
            {
                var head = CreateOrderedCircularList(keyList, skiplist.CreateNode);

                while (head.LengthOfList() > MaximumGapSize)
                {
                    head = CreateNextLevel(head, skiplist.CreateNode);
                }

                skiplist.Head = head;
            }


            return skiplist;
        }

        private static INode<T> CreateOrderedCircularList<T>(IEnumerable<T> keys, Func<T, INode<T>> nodeFactory)
        {
            var orderedKeys = keys.OrderBy(key => key);
            var nodes = orderedKeys.Select(nodeFactory).ToList();

            var previousNode = nodes.First();
            foreach (var node in nodes.Skip(1))
            {
                previousNode.ConnectTo(node);
                previousNode = node;
            }

            nodes.Last().ConnectTo(nodes.First());

            return nodes.First();
        }

        private static INode<T> CreateNextLevel<T>(INode<T> oldHead, Func<T, INode<T>> nodeFactory)
        {
            var head = nodeFactory(oldHead.Key);
            head.ConnectDownTo(oldHead);

            var node = head;
            while (node.Down.DistanceRightTo(oldHead) >= 2*MaximumGapSize)
            {
                var nodeBelowNewNode = node.Down.Right(MaximumGapSize);
                node = CreateNodeAdjacentTo(node, nodeBelowNewNode, nodeFactory);
            }

            if (node.Down.DistanceRightTo(oldHead) > MaximumGapSize)
            {
                var nodeBelowNewNode = node.Down.Right(node.Down.DistanceRightTo(oldHead) / 2);
                node = CreateNodeAdjacentTo(node, nodeBelowNewNode, nodeFactory);
            }

            node.ConnectTo(head);

            return head;
        }

        private static INode<T> CreateNodeAdjacentTo<T>(INode<T> nodeToLeft, INode<T> nodeBelow, Func<T, INode<T>> nodeFactory)
        {
            var newNode = nodeFactory(nodeBelow.Key);

            nodeToLeft.ConnectTo(newNode);         
            newNode.ConnectDownTo(nodeBelow);

            return newNode;
        }

        //private static INode<T> CreateNode<T>(T key)
        //{
        //    return new Node<T> {Key = key};
        //}
    }
}
