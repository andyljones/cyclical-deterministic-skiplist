using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistTests
    {
        [Theory, AutoNSubData]
        public void ReferenceNode_ShouldAcceptAnINodeInstance(Skiplist<int> sut, INode<int> anonymousNode)
        {
            // Fixture setup
            // Exercise system
            sut.ReferenceNode = anonymousNode;
            // Verify outcome
            Assert.NotNull(sut);
            // Teardown
        }

        [Theory, AutoNSubData]
        public void GetEnumeratorINodeT_WhenSkiplistIsSinglyLinked_ShouldEnumerateAllNodes(Skiplist<int> sut, INode<int>[] nodes)
        {
            // Fixture setup
            nodes[0].Right = nodes[1];
            nodes[1].Right = nodes[2];
            nodes[2].Right = null;

            sut.ReferenceNode = nodes[0];
            // Exercise system
            var results = new List<INode<int>>();
            foreach (var node in sut)
            {
                results.Add(node);
            }
            // Verify outcome
            Assert.Equal(nodes, results);
            // Teardown
        }

        [Theory, AutoNSubData]
        public void Skiplist_ShouldImplementIEnumerableINodeT(Skiplist<int> sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut is IEnumerable<INode<int>>;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Theory, AutoNSubData]
        public void Add_OnAnEmptyList_ShouldCreateASingleElementSkiplist(Skiplist<int> sut, INode<int> node)
        {
            // Fixture setup
            node.Right = null;
            // Exercise system
            sut.Add(node);
            // Verify outcome
            var results = sut.ToArray();
            Assert.Equal(node, results.Single());
            // Teardown
        }
    }
}
