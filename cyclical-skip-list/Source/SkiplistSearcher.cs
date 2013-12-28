using System;

namespace CyclicalSkipList
{
    public static class SkiplistSearcher
    {
        public static bool Find<T>(
            this Skiplist<T> skiplist, 
            T key, 
            INode<T> origin = null, 
            Action<INode<T>> pathAction = null)
        {
            var nodeOfKey = skiplist.FetchNode(key, origin, pathAction);

            return skiplist.AreEqual(nodeOfKey.Key, key);
        }

        public static INode<T> FetchNode<T>(
            this Skiplist<T> skiplist, 
            T key, 
            INode<T> origin = null,
            Action<INode<T>> pathAction = null)
        {
            if (skiplist.Head == null)
            {
                return null;
            }

            var node = origin ?? skiplist.Head;

            var atCorrectNode = false;
            while (!atCorrectNode)
            {
                node = FindCorrectGapInLevel(key, node, skiplist.InOrder, skiplist.AreEqual);

                if (pathAction != null)
                {
                    pathAction(node);
                }

                if (node.Down != null)
                {
                    node = node.Down;
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return node;
        }

        private static INode<T> FindCorrectGapInLevel<T>(T key, INode<T> start, Func<T, T, T, bool> inOrder, Func<T, T, bool> areEqual)
        {
            var node = start;

            while (!inOrder(node.Key, key, node.Right.Key))
            {
                node = node.Right;
                if (node == start) { break; }
            }

            if (node.Down == null)
            {
                while (!areEqual(node.Key, key) && inOrder(node.Right.Key, key, node.Right.Right.Key))
                {
                    node = node.Right;
                    if (node == start) { break; }
                }
            }

            return node;
        }
    }
}
