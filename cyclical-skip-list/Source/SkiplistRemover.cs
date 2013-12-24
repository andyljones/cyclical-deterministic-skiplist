using System;

namespace CyclicalSkipList
{
    public static class SkiplistRemover
    {
        public static bool Remove<T>(this Skiplist<T> skiplist, T key)
        {
            if (skiplist.Head.Down == null && Equals(skiplist.Head.Key, key))
            {
                RemoveHead(skiplist);
            }

            var result = skiplist.Find(key, node => RemoverAction(key, skiplist, node));

            //TODO: Test
            if (skiplist.Head.Down != null && skiplist.Head.Right == skiplist.Head)
            {
                skiplist.Head = skiplist.Head.Down;
                skiplist.Head.Up = null;
            }
        }

        //TODO: Test
        private static void RemoveHead<T>(Skiplist<T> skiplist)
        {
            var oldHead = skiplist.Head;
            if (oldHead.Right != oldHead)
            {
                var newHead = oldHead.Right;
                oldHead.Left.ConnectTo(newHead);
                skiplist.Head = newHead;
            }
            else
            {
                skiplist.Head = null;
            }
        }

        private static void RemoverAction<T>(T key, Skiplist<T> skiplist, INode<T> node)
        {
            if (node.Down == null && Equals(node.Key, key))
            {
                RemoveAtBottom(node);
            }
            else if (node.SizeOfGap() <= skiplist.MinimumGapSize)
            {
                ExpandGap(node, skiplist);
            }
        }

        private static void RemoveAtBottom<T>(INode<T> node)
        {
            if (node.Up != null)
            {
                node.Up.ConnectDownTo(node.Left);
                SetKeysInColumn(node.Left);
            }

            node.Left.ConnectTo(node.Right);
        }

        private static void SetKeysInColumn<T>(INode<T> column)
        {
            var bottomNode = column.Bottom();
            var bottomKey = bottomNode.Key;

            var node = bottomNode;
            while (node.Up != null)
            {
                node = node.Up;
                node.Key = bottomKey;
            }
        }

        private static void ExpandGap<T>(INode<T> node, Skiplist<T> skiplist)
        {
            if (node.Right.SizeOfGap() > skiplist.MinimumGapSize)
            {
                BorrowFromNextGap(node);
            } 
            else if (node.Right.SizeOfGap() <= skiplist.MinimumGapSize)
            {
                MergeWithNextGap(node);
            }
        }

        private static void MergeWithNextGap<T>(INode<T> node)
        {
            var nextNode = node.Right;

            if (nextNode.Up != null)
            {
                nextNode.Up.ConnectDownTo(nextNode.Right);
                nextNode.Up = null;
            }
            if (nextNode.Down != null)
            {
                nextNode.Right.ConnectDownTo(nextNode.Down);
                nextNode.Down = null;
            }

            node.ConnectTo(nextNode.Right);

            SetKeysInColumn(nextNode);
        }

        private static void BorrowFromNextGap<T>(INode<T> node)
        {
            var gapEnd = node.Right.Down;

            if (gapEnd.Up != null)
            {
                gapEnd.Up.ConnectDownTo(gapEnd.Right);
                gapEnd.Up = null;
            }
            if (gapEnd.Down != null)
            {
                gapEnd.Right.ConnectDownTo(gapEnd.Down);
                gapEnd.Down = null;
            }

            SetKeysInColumn(gapEnd.Right);
        }
    }
}
