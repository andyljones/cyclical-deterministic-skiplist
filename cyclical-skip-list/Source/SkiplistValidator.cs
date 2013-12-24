using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistValidator
    {
        public static IEnumerable<INode<T>> ReachableNodes<T>(Skiplist<T> skiplist)
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

        public static bool ValidateRightwardsReachability<T>(Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var allReachableNodes = ReachableNodes(skiplist);

            var headsOfLevels = skiplist.Head.EnumerateDown();
            var nodesReachableGoingRightwards = headsOfLevels.SelectMany(headOfLevel => headOfLevel.EnumerateRight());

            failingNodes = allReachableNodes.Except(nodesReachableGoingRightwards);

            return !failingNodes.Any();
        }

        public static bool ValidateOrdering<T>(Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var nodes = ReachableNodes(skiplist);

            failingNodes = nodes.Where(node => !skiplist.Compare(node.Left.Key, node.Key, node.Right.Key));

            return !failingNodes.Any();
        }

        public static bool ValidateGapSize<T>(Skiplist<T> skiplist, out IEnumerable<INode<T>> failingNodes)
        {
            var upperNodes = ReachableNodes(skiplist).Where(node => node.Down != null);

            failingNodes = upperNodes.Where( node => node.SizeOfGap() < skiplist.MinimumGapSize || 
                                                     node.SizeOfGap() > skiplist.MaximumGapSize);

            return !failingNodes.Any();
        }

    }
}
