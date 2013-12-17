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

        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

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
            var currentNode = Head;
            bool atCorrectNode = false;
            while (!atCorrectNode)
            {
                currentNode = FindGapContainingKey(key, currentNode);

                if (currentNode.Down != null)
                {
                    currentNode = currentNode.Down;    
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return (_equals(key, currentNode.Key)); 
        }

        private INode<T> FindGapContainingKey(T key, INode<T> nodeInLevel)
        {
            var ends = SkiplistUtilities.EnumerateLevels(Head).ToList();

            var currentNode = nodeInLevel;
            while (_compare(key, currentNode.Key) > 0 && !ends.Contains(currentNode.Right))
            {
                currentNode = currentNode.Right;
            }

            return currentNode;
        }

        public void Insert(T key)
        {
            if (Head == null)
            {
                CreateNewHead(key);
                return;
            }

            var nodeToInsertAt = FindNodeContainingKey(key);
            InsertKey(key, nodeToInsertAt);

            if (Head.Right != Head)
            {
                IncreaseSkiplistHeight();
            }
        }

        private void CreateNewHead(T key)
        {
            Head = new Node<T>(key);
            Head.ConnectTo(Head);
        }

        private INode<T> FindNodeContainingKey(T key)
        {
            var currentNode = Head;
            bool atCorrectNode = false;
            while (!atCorrectNode)
            {
                currentNode = FindGapContainingKey(key, currentNode);

                if (currentNode.Down != null && currentNode.SizeOfGap() >= MaximumGapSize)
                {
                    SplitGap(currentNode);
                }

                if (currentNode.Down != null)
                {
                    currentNode = currentNode.Down;
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return currentNode;
        }

        private void SplitGap(INode<T> currentNode)
        {
            var gapSize = currentNode.SizeOfGap();
            var gapMidpoint = currentNode.Down.RightBy(gapSize/2);

            var newNode = new Node<T>(currentNode.Key);
            newNode.ConnectDownTo(gapMidpoint);
            newNode.ConnectTo(currentNode.Right);
            currentNode.ConnectTo(newNode);
            currentNode.Key = currentNode.Down.RightBy(gapSize/2 - 1).Key;

        }

        private void InsertKey(T newKey, INode<T> node)
        {
            var newNode = new Node<T>(node.Key);

            node.Key = newKey;
            newNode.ConnectTo(node.Right);            
            node.ConnectTo(newNode);
        }

        private void IncreaseSkiplistHeight()
        {
            var oldHead = Head;
            CreateNewHead(oldHead.Right.Key);
            Head.ConnectDownTo(oldHead);
        }

        public override string ToString()
        {
            return SkiplistStringFormatter.ConvertToString(Head);
        }

        public void Remove(T key)
        {
            var atCorrectNode = false;
            var currentNode = Head.Down;
            while (!atCorrectNode)
            {
                currentNode = FindGapContainingKey(key, currentNode);

                var isBottom = currentNode.Down == null;

                if (isBottom)
                {
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
            bool containsKey = _equals(node.Key, key);
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
            while(node.Right.Up != null)
            {
                node = node.Right.Up.Left;
                node.Key = node.Right.Down.Left.Key;
            }
        }
    }
}
