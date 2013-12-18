using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistInserter
    {
        public static void Insert<T>(this Skiplist<T> skiplist, T key)
        {
            if (skiplist.Head == null)
            {
                skiplist.CreateNewHead(key);
                return;
            }

            var nodeToInsertAt = skiplist.FindNodeContainingKey(key);
            skiplist.InsertKey(key, nodeToInsertAt);

            if (skiplist.Head.Right != skiplist.Head)
            {
                skiplist.IncreaseSkiplistHeight();
            }
        }

        private static void CreateNewHead<T>(this Skiplist<T> skiplist, T key)
        {
            skiplist.Head = new Node<T>(key);
            skiplist.Head.ConnectTo(skiplist.Head);
        }

        private static INode<T> FindNodeContainingKey<T>(this Skiplist<T> skiplist, T key)
        {
            var currentNode = skiplist.Head;
            bool atCorrectNode = false;
            while (!atCorrectNode)
            {
                currentNode = skiplist.FindGapContainingKey(key, currentNode);

                if (currentNode.Down != null && currentNode.SizeOfGap() >= skiplist.MaximumGapSize)
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

        private static void SplitGap<T>(INode<T> currentNode)
        {
            var gapSize = currentNode.SizeOfGap();
            var gapMidpoint = currentNode.Down.RightBy(gapSize / 2);

            var newNode = new Node<T>(currentNode.Key);
            newNode.ConnectDownTo(gapMidpoint);
            newNode.ConnectTo(currentNode.Right);
            currentNode.ConnectTo(newNode);
            currentNode.Key = currentNode.Down.RightBy(gapSize / 2 - 1).Key;

        }

        private static void InsertKey<T>(this Skiplist<T> skiplist, T newKey, INode<T> node)
        {
            var newNode = new Node<T>(node.Key);

            node.Key = newKey;
            newNode.ConnectTo(node.Right);
            node.ConnectTo(newNode);
        }

        private static void IncreaseSkiplistHeight<T>(this Skiplist<T> skiplist)
        {
            var oldHead = skiplist.Head;
            skiplist.CreateNewHead(oldHead.Right.Key);
            skiplist.Head.ConnectDownTo(oldHead);
        }
    }
}
