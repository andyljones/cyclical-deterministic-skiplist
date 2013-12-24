using System;

namespace CyclicalSkipList
{
    public static class SkiplistInserter
    {
        public static void Insert<T>(this Skiplist<T> skiplist, T key)
        {
            skiplist.Find(key, node => InserterAction(key, node, skiplist));
        }

        private static void InserterAction<T>(T key, INode<T> pathNode, Skiplist<T> skiplist)
        {
            if (pathNode.Down == null)
            {
                InsertKeyAfterNode(key, pathNode, skiplist.CreateNode);
            }
            else if (pathNode.SizeOfGap() > skiplist.MaximumGapSize)
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
