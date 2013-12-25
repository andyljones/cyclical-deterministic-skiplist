using System;

namespace CyclicalSkipList
{
    public static class SkiplistInserter
    {
        public static void Insert<T>(this Skiplist<T> skiplist, T key)
        {
            if (skiplist.Head == null)
            {
                //TODO: Test.
                skiplist.Head = skiplist.CreateNode(key);
                skiplist.Head.ConnectTo(skiplist.Head);
                return;
            }


            if (skiplist.Head.DistanceRightTo(skiplist.Head) >= skiplist.MaximumGapSize)
            {
                //TODO: Test
                var newHead = skiplist.CreateNode(skiplist.Head.Key);
                newHead.ConnectTo(newHead);
                newHead.ConnectDownTo(skiplist.Head);

                skiplist.Head = newHead;
            }

            skiplist.Find(key, pathAction: node => InserterAction(key, skiplist, node));
        }

        private static void InserterAction<T>(T key, Skiplist<T> skiplist, INode<T> pathNode)
        {
            if (pathNode.Down == null)
            {
                InsertKeyAfterNode(key, pathNode, skiplist.CreateNode);
            }
            else if (pathNode.SizeOfGap() >= skiplist.MaximumGapSize)
            {
                SplitGap(pathNode, skiplist.CreateNode);
            }
        }

        private static void InsertKeyAfterNode<T>(T key, INode<T> pathNode, Func<T, INode<T>> nodeFactory)
        {
            var newNode = nodeFactory(key);
            newNode.ConnectTo(pathNode.Right);
            pathNode.ConnectTo(newNode);
        }

        private static void SplitGap<T>(INode<T> node, Func<T, INode<T>> nodeFactory)
        {
            var sizeOfGap = node.SizeOfGap();
            var nodeToSplitAt = node.Down.Right(sizeOfGap/2);

            var newNode = nodeFactory(nodeToSplitAt.Key);
            newNode.ConnectDownTo(nodeToSplitAt);
            newNode.ConnectTo(node.Right);
            node.ConnectTo(newNode);
        }
    }
}
