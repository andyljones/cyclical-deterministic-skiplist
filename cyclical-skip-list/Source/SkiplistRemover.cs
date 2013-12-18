namespace CyclicalSkipList
{
    public static class SkiplistRemover
    {
        public static void Remove<T>(this Skiplist<T> skiplist, T key)
        {
            var atCorrectNode = false;
            var currentNode = skiplist.Head.Down; //TODO: Will error on single-node lists.
            while (!atCorrectNode)
            {
                currentNode = skiplist.FindGapContainingKey(key, currentNode);

                var isBottom = currentNode.Down == null;

                if (isBottom)
                {
                    currentNode = skiplist.FindGapContainingKey(key, currentNode);
                    atCorrectNode = true;
                    skiplist.TryRemoveBottomNode(currentNode, key);
                }
                else
                {
                    skiplist.AmendGapSize(currentNode);
                    currentNode = currentNode.Down;

                }
            }

            if (skiplist.Head.Down.Right == skiplist.Head.Down)
            {
                skiplist.Head.Down.Up = null;
                skiplist.Head = skiplist.Head.Down;
            }
        }

        private static void AmendGapSize<T>(this Skiplist<T> skiplist, INode<T> nodeOfGap)
        {
            bool needsExpanding = nodeOfGap.SizeOfGap() <= skiplist.MinimumGapSize;
            bool isLastGap = nodeOfGap.Right.Up != null;

            if (needsExpanding && !isLastGap)
            {
                skiplist.ExpandIntoNextGap(nodeOfGap);
            }
            else if (needsExpanding && isLastGap)
            {
                skiplist.ExpandIntoPreviousGap(nodeOfGap);
            }
        }

        private static void ExpandIntoPreviousGap<T>(this Skiplist<T> skiplist, INode<T> node)
        {
            var nodeOfPreviousGap = node.Left;
            bool shouldMergeGaps = nodeOfPreviousGap.SizeOfGap() <= skiplist.MinimumGapSize;

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

        private static void ExpandIntoNextGap<T>(this Skiplist<T> skiplist, INode<T> node)
        {
            var nodeOfNextGap = node.Right;
            bool shouldMergeGaps = nodeOfNextGap.SizeOfGap() <= skiplist.MinimumGapSize;

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

        private static void TryRemoveNode<T>(INode<T> node)
        {
            node.Down.Up = null;
            node.Left.ConnectTo(node.Right);

            node.Left.Key = node.Key;
        }

        private static void TryRemoveBottomNode<T>(this Skiplist<T> skiplist, INode<T> node, T key)
        {
            bool containsKey = skiplist.KeyEquals(node.Key, key);
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

        private static void CleanNodeFromSkiplist<T>(INode<T> node)
        {
            while (node.Right.Up.Left != null)
            {
                node = node.Right.Up.Left;
                node.Key = node.Right.Down.Left.Key;
            }
        }
    }
}
