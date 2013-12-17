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
            var currentNode = Head;
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
                    currentNode = DescendIntoGap(currentNode);
                }
            }
        }

        private INode<T> DescendIntoGap(INode<T> node)
        {
            return node.Down;
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
                node.Key = node.Left.Key;
                node.Left.Left.ConnectTo(node);
            }
        }
    }
}
