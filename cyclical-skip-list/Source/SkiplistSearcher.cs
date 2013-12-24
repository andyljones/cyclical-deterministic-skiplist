using System;

namespace CyclicalSkipList
{
    public static class SkiplistSearcher
    {
        public static bool Find<T>(this Skiplist<T> skiplist, T key, Action<INode<T>> pathAction = null)
        {
            if (skiplist.Head == null)
            {
                return false;
            }

            var node = skiplist.Head;

            var atCorrectNode = false;
            while (!atCorrectNode)
            {
                node = FindCorrectGapInLevel(key, node, skiplist.Compare);

                if (node.Down != null)
                {
                    node = node.Down;
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return Equals(node.Key, key);
        }

        private static INode<T> FindCorrectGapInLevel<T>(T key, INode<T> start, Func<T, T, T, bool> compare)
        {
            var node = start;

            while (!compare(node.Key, key, node.Right.Key))
            {
                node = node.Right;
            }

            return node;
        }
    }
}
