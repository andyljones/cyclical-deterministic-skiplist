using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistFactory
    {
        public static int MinimumGapSize = 2;
        public static int MaximumGapSize = 4;

        public static Skiplist<T> CreateFrom<T>(IEnumerable<T> keys)
        {
            var skiplist = new Skiplist<T>();

            if (keys.Any())
            {
                var head = new Node<T>();
                head.ConnectTo(head);
                skiplist.Head = head;
            }

            return skiplist;
        }
    }
}
