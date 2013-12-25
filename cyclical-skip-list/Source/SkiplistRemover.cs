namespace CyclicalSkipList
{
    public static class SkiplistRemover
    {
        public static bool Delete<T>(this Skiplist<T> skiplist, T key)
        {
            if (skiplist.Head == null)
            {
                return false;
            }

            if (skiplist.Head.Down == null && Equals(skiplist.Head.Key, key))
            {
                return TryRemoveCurrentHead(skiplist, key);
            }

            var keyFound = 
                skiplist.Find(
                key: key, 
                pathAction: node => HandlePathToDeletionPoint(node, key, skiplist.MinimumGapSize), 
                origin: skiplist.Head.Down);

            if (skiplist.Head.Down.LengthOfList() <= 1)
            {
                skiplist.Head = skiplist.Head.Down;
                skiplist.Head.Up = null;
            }

            return keyFound;
        }

        private static bool TryRemoveCurrentHead<T>(Skiplist<T> skiplist, T key)
        {
            if (skiplist.Head.Right == skiplist.Head && Equals(skiplist.Head.Key, key))
            {
                skiplist.Head = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void HandlePathToDeletionPoint<T>(INode<T> node, T key, int minimumGapSize)
        {
            if (node.Down != null && node.SizeOfGap() <= minimumGapSize)
            {
                ExpandGap(node, minimumGapSize);
            }
            else if (node.Down == null && Equals(node.Key, key))
            {
                RemoveNodeFromBottomLevel(node);
            }

        }

        private static void ExpandGap<T>(INode<T> headOfGap, int minimumGapSize)
        {
            if (headOfGap.Right.SizeOfGap() > minimumGapSize)
            {
                headOfGap.Right.Down.Up = null;
                headOfGap.Right.ConnectDownTo(headOfGap.Right.Down.Right);
                UpdateKeysInColumn(headOfGap.Right);
            }
            else if (headOfGap.Right.Up == null)
            {
                headOfGap.Right.Down.Up = null;
                headOfGap.ConnectTo(headOfGap.Right.Right);
            }
            else
            {
                headOfGap.Down.Up = null;
                headOfGap.Left.ConnectTo(headOfGap.Right);
            }
        }

        private static void RemoveNodeFromBottomLevel<T>(INode<T> node)
        {
            node.Left.ConnectTo(node.Right);

            if (node.Up != null)
            {
                node.Up.ConnectDownTo(node.Right);
                UpdateKeysInColumn(node.Right);
            }
        }

        private static void UpdateKeysInColumn<T>(INode<T> column)
        {
            var bottomOfColumn = column.Bottom();

            var node = bottomOfColumn;
            while (node.Up != null)
            {
                node = node.Up;
                node.Key = bottomOfColumn.Key;
            }
        }
    }
}
