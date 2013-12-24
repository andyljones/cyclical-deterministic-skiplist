using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistValidator
    {
        public static IEnumerable<INode<T>> ReachableNodes<T>(this Skiplist<T> skiplist)
        {
            var discoveredNodes = new HashSet<INode<T>>();
            var processedNodes = new HashSet<INode<T>>();

            if (skiplist.Head != null)
            {
                discoveredNodes.Add(skiplist.Head);
            }

            while (discoveredNodes.Any())
            {
                var node = discoveredNodes.First();
                var neighbours = new HashSet<INode<T>> {node.Left, node.Right, node.Up, node.Down};

                neighbours.RemoveWhere(item => item == null || processedNodes.Contains(item));
                discoveredNodes.UnionWith(neighbours);

                discoveredNodes.Remove(node);
                processedNodes.Add(node);
            }

            return processedNodes;
        }

        public static bool ValidateRightwardsReachability<T>(this Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var allReachableNodes = skiplist.ReachableNodes();

            var headsOfLevels = skiplist.Head.EnumerateDown();
            var nodesReachableGoingRightwards = headsOfLevels.SelectMany(headOfLevel => headOfLevel.EnumerateRight());

            failingNodes = allReachableNodes.Except(nodesReachableGoingRightwards);

            return !failingNodes.Any();
        }

        public static bool ValidateOrdering<T>(this Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var headsOfLevels = skiplist.Head.EnumerateDown();

            failingNodes = headsOfLevels.SelectMany(headOfLevel => OutOfOrderNodesInLevel(headOfLevel, skiplist.Compare));

            return !failingNodes.Any();
        }

        private static IEnumerable<INode<T>> OutOfOrderNodesInLevel<T>(INode<T> headOfLevel, Func<T, T, T, bool> compare)
        {
            var nodesInLevel = headOfLevel.EnumerateRight();

            var nodesOutOfOrder = nodesInLevel.Where(node => !compare(node.Left.Key, node.Key, node.Right.Key));

            return nodesOutOfOrder;
        }

        public static bool ValidateGapSize<T>(this Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var upperNodes = skiplist.ReachableNodes().Where(node => node.Down != null);

            failingNodes = upperNodes.Where( node => node.SizeOfGap() < skiplist.MinimumGapSize || 
                                                     node.SizeOfGap() > skiplist.MaximumGapSize);

            return !failingNodes.Any();
        }

    }
}
