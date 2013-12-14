using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CyclicalSkipList;

namespace CyclicalSkipListTests
{
    public static class Utilities
    {
        public static INode<T> BottomOf<T>(INode<T> node)
        {
            var currentNode = node;
            while (currentNode.Down != null)
            {
                currentNode = currentNode.Down;
            }

            return currentNode;
        }

        public static int DistanceTo<T>(this INode<T> start, INode<T> end)
        {
            var distance = 0;

            var currentNode = start;
            while (currentNode != end)
            {
                distance++;
                currentNode = currentNode.Right;
            }

            return distance;
        }

        public static INode<T> RightBy<T>(this INode<T> start, int count)
        {
            var currentNode = start;
            while (count > 0)
            {
                currentNode = currentNode.Right;
                count--;
            }

            return currentNode;
        }

        public static INode<T> Left<T>(this INode<T> start)
        {
            var currentNode = start;
            while (currentNode.Right != start)
            {
                currentNode = currentNode.Right;
            }

            return currentNode;
        }

        public static int Count<T>(this INode<T> node)
        {
            return node.Right.DistanceTo(node) + 1;
        }

        public static void InsertToRight<T>(this INode<T> node, INode<T> nodeToBeInserted)
        {
            nodeToBeInserted.Right = node.Right;
            node.Right = nodeToBeInserted;
        }

        public static int DistanceAlongSublevelTo<T>(this INode<T> start, INode<T> end)
        {
            return start.Down.DistanceTo(end.Down);
        }
    }
}
