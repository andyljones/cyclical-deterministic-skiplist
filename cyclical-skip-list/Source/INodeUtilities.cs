using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CyclicalSkipList
{
    public static class INodeUtilities
    {
        public static void ConnectTo<T>(this INode<T> left, INode<T> right)
        {
            if (left != null)
            {
                left.Right = right;
            }
            if (right != null)
            {
                right.Left = left;
            }
        }

        public static void ConnectDownTo<T>(this INode<T> top, INode<T> bottom)
        {
            if (top != null)
            {
                top.Down = bottom;
            }
            if (bottom != null)
            {
                bottom.Up = top;
            }
        }

        public static IEnumerable<INode<T>> EnumerateRight<T>(this INode<T> start)
        {
            var node = start;
            do
            {
                yield return node;
                node = node.Right;
            } while (node != start && node != null);
        }

        public static IEnumerable<INode<T>> EnumerateDown<T>(this INode<T> start)
        {
            var node = start;
            do
            {
                yield return node;
                node = node.Down;
            } while (node != null);
        }

        public static INode<T> Bottom<T>(this INode<T> start)
        {
            return start.EnumerateDown().Last();
        }

        public static INode<T> Right<T>(this INode<T> start, int count)
        {
            return start.EnumerateRight().Take(count + 1).Last();
        }

        public static int DistanceRightTo<T>(this INode<T> origin, INode<T> destination)
        {
            var distance = 1;
            var node = origin.Right;
            while (node != origin && node != destination)
            {
                node = node.Right;
                distance++;
            }

            if (origin != destination && node == origin)
            {
                throw new InvalidDataException(String.Format("Returned to origin {0} without finding destination {1}!", origin, destination));
            }
        
            return distance;
        }

        public static int LengthOfList<T>(this INode<T> node)
        {
            return node.EnumerateRight().Count();
        }

        public static int SizeOfGap<T>(this INode<T> node)
        {
            if (node.Down == null)
            {
                throw new ArgumentException("Node does not preceed a gap!");
            }
            else if (node.Right.Down == null)
            {
                throw new ArgumentException("Node to the right does not suceed a gap!");
            }
            
            return node.Down.DistanceRightTo(node.Right.Down);
        }

        public static int Height<T>(this INode<T> node)
        {
            if (node.Down == null)
            {
                return 0;
            }
            else
            {
                return node.Down.Height() + 1;
            }
        }
    }
}
