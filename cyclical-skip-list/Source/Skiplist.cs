﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head { get; set; }

        public readonly Func<T, T, int> KeyCompare;
        public readonly Func<T, T, bool> KeyEquals;

        public readonly int MinimumGapSize = 2;
        public readonly int MaximumGapSize = 4;

        public Skiplist()
        {
            KeyCompare = Comparer<T>.Default.Compare;
            KeyEquals = EqualityComparer<T>.Default.Equals;
        }

        public Skiplist(INode<T> head) : this()
        {
            Head = head;
        }

        public override string ToString()
        {
            return SkiplistStringFormatter.ConvertToString(Head);
        }

        public void Remove(T key)
        {
            var atCorrectNode = false;
            var currentNode = Head.Down; //TODO: Will error on single-node lists.
            while (!atCorrectNode)
            {
                currentNode = this.FindGapContainingKey(key, currentNode);

                var isBottom = currentNode.Down == null;

                if (isBottom)
                {
                    currentNode = this.FindGapContainingKey(key, currentNode);
                    atCorrectNode = true;
                    TryRemoveBottomNode(currentNode, key);
                }
                else
                {
                    AmendGapSize(currentNode);
                    currentNode = currentNode.Down;

                }
            }

            if (Head.Down.Right == Head.Down)
            {
                Head.Down.Up = null;
                Head = Head.Down;
            }
        }

        private void AmendGapSize(INode<T> nodeOfGap)
        {
            bool needsExpanding = nodeOfGap.SizeOfGap() <= MinimumGapSize;
            bool isLastGap = nodeOfGap.Right.Up != null;

            if (needsExpanding && !isLastGap)
            {
                ExpandIntoNextGap(nodeOfGap);
            }
            else if (needsExpanding && isLastGap)
            {
                ExpandIntoPreviousGap(nodeOfGap);
            }
        }

        private void ExpandIntoPreviousGap(INode<T> node)
        {
            var nodeOfPreviousGap = node.Left;
            bool shouldMergeGaps = nodeOfPreviousGap.SizeOfGap() <= MinimumGapSize;

            if (shouldMergeGaps)
            {
                TryRemoveNode(node);
            }
            else
            {
                nodeOfPreviousGap.Key = node.Down.Left.Left.Key;
                node.Down.Up = null;
                node.ConnectDownTo(node.Down.Left);
            }
        }

        private void ExpandIntoNextGap(INode<T> node)
        {
            var nodeOfNextGap = node.Right;
            bool shouldMergeGaps = nodeOfNextGap.SizeOfGap() <= MinimumGapSize;

            if (shouldMergeGaps)
            {
                TryRemoveNode(nodeOfNextGap);
            }
            else
            {
                node.Key = nodeOfNextGap.Down.Key;
                nodeOfNextGap.Down.Up = null;
                nodeOfNextGap.ConnectDownTo(nodeOfNextGap.Down.Right);
            }
        }

        private void TryRemoveNode(INode<T> node)
        {
            node.Down.Up = null;
            node.Left.ConnectTo(node.Right);

            node.Left.Key = node.Key;
        }

        private void TryRemoveBottomNode(INode<T> node, T key)
        {
            bool containsKey = KeyEquals(node.Key, key);
            bool isLastNodeInGap = node.Right.Up != null;

            if (containsKey && !isLastNodeInGap)
            {
                node.Key = node.Right.Key;
                node.ConnectTo(node.Right.Right);
            } 
            else if (containsKey && isLastNodeInGap)
            {
                node.Left.ConnectTo(node.Right);
                CleanNodeFromSkiplist(node);
            }
        }

        private void CleanNodeFromSkiplist(INode<T> node)
        {
            while(node.Right.Up.Left != null)
            {
                node = node.Right.Up.Left;
                node.Key = node.Right.Down.Left.Key;
            }
        }
    }
}
