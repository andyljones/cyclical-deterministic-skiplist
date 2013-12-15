using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistFactory
    {
        private const int MinimumGapSize = 1;
        private const int MaximumGapSize = 4;

        public static Skiplist<T> CreateFrom<T>(IEnumerable<T> keys)
        {
            var keyList = keys.ToList();

            if (keyList.Count == 0)
            {
                return null;
            }

            var headOfTopLevel = CreateLinkedListFrom(keyList);

            while (headOfTopLevel.DistanceToSelf() > MaximumGapSize)
            {
                headOfTopLevel = AddLevelToSkiplist(headOfTopLevel);
            }

            return new Skiplist<T>(headOfTopLevel);
        }

        private static INode<T> AddLevelToSkiplist<T>(INode<T> currentHead)
        {
            var newHead = new Node<T>(default(T)) { Down = currentHead };

            var currentNewLevelNode = newHead;
            var currentOldLevelNode = newHead.Down;
            
            //TODO: Can this be rewritten into a for loop?
            var sizeOfRemainingGap = currentNewLevelNode.SizeOfGapTo(newHead);
            while (sizeOfRemainingGap > 2*MaximumGapSize)
            {
                currentOldLevelNode = currentOldLevelNode.RightBy(MaximumGapSize);
                var newFirstLevelNode = new Node<T>(default(T)) { Down = currentOldLevelNode };
                currentNewLevelNode.Right = newFirstLevelNode;

                currentNewLevelNode = newFirstLevelNode;
                sizeOfRemainingGap = currentNewLevelNode.SizeOfGapTo(newHead);
            }

            if (sizeOfRemainingGap > MaximumGapSize)
            {
                currentOldLevelNode = currentOldLevelNode.RightBy(sizeOfRemainingGap / 2);
                var newFirstLevelNode = new Node<T>(default(T)) { Down = currentOldLevelNode };
                currentNewLevelNode.Right = newFirstLevelNode;

                currentNewLevelNode = newFirstLevelNode;
            }

            currentNewLevelNode.Right = newHead;
            AssignKeysToLevel(newHead);

            return newHead;
        }

        private static void AssignKeysToLevel<T>(INode<T> head)
        {
            foreach (var node in SkiplistUtilities.EnumerateNodesInLevel(head))
            {
                node.Key = node.Right.Down.Left().Key;
            }
        }

        private static INode<T> CreateLinkedListFrom<T>(IEnumerable<T> keys)
        {
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
