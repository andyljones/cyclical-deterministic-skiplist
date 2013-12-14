using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace CyclicalSkipListTests
{
    public class UtilitiesTests
    {
        private const int LowerLengthBound = 3;
        private const int UpperLengthBound = 10;

        [Theory]
        [AutoIsolatedNodeData(LowerLengthBound, UpperLengthBound)]
        public void BottomOf_GivenAVerticalLinkedList_ReturnsTheBottomNode
            (List<INode<int>> nodes)
        {
            // Fixture setup
            if (nodes.Count > 1)
            {
                for (int i = 1; i < nodes.Count; i++)
                {
                    nodes[i].Down = nodes[i - 1];
                }
            }

            var sutHead = nodes.Last();

            // Exercise system
            var result = Utilities.BottomOf(sutHead);

            // Verify outcome
            Assert.Equal(nodes[0], result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceTo_GivenAHorizontalLinkedList_ReturnsTheDistanceBetweenTheSpecifiedNodes
            (List<INode<int>> nodes, INode<int> sutHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = sutHead.DistanceTo(nodes[0]);
            var result1 = sutHead.DistanceTo(nodes[1 % nodes.Count]);
            var result2 = sutHead.DistanceTo(nodes[nodes.Count - 1]);

            // Verify outcome
            Assert.Equal(0 % nodes.Count, result0);
            Assert.Equal(1 % nodes.Count, result1);
            Assert.Equal(nodes.Count - 1, result2);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void LengthOf_GivenAHorizontalLinkedList_ReturnsTheLengthOfTheList
            (List<INode<int>> nodes, INode<int> sutHead)
        {
            // Fixture setup

            // Exercise system
            var result = sutHead.Count();

            // Verify outcome
            Assert.Equal(nodes.Count, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void MoveRightBy_GivenAHorizontalLinkedList_ReturnsANodeTheCorrectDistanceToTheRightOfSpecifiedNode
            (List<INode<int>> nodes, INode<int> sutHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = sutHead.RightBy(0);
            var result1 = sutHead.RightBy(2);
            var result2 = sutHead.RightBy(nodes.Count + 1);

            // Verify outcome
            Assert.Equal(nodes[0 % nodes.Count], result0);
            Assert.Equal(nodes[2 % nodes.Count], result1);
            Assert.Equal(nodes[1 % nodes.Count], result2);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void Left_OnAHorizontalLinkedList_ReturnsTheNodeLeftOfTheSpecifiedNode
            (List<INode<int>> nodes, INode<int> sutHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = nodes[0 % nodes.Count].Left();
            var result1 = nodes[2 % nodes.Count].Left();
            var result2 = nodes[nodes.Count - 1].Left();

            // Verify outcome
            Assert.Equal(nodes[nodes.Count - 1], result0);
            Assert.Equal(nodes[1 % nodes.Count], result1);
            Assert.Equal(nodes[(nodes.Count - 2) % nodes.Count], result2);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData]
        public void InsertToRight_WhenTwoNodesAreLinked_InsertsAnotherNodeInBetweenThem
            (INode<int> firstNode, INode<int> lastNode, INode<int> nodeToBeInserted)
        {
            // Fixture setup
            firstNode.Right = lastNode;

            // Exercise system
            firstNode.InsertToRight(nodeToBeInserted);

            // Verify outcome
            Assert.Equal(nodeToBeInserted, firstNode.Right);
            Assert.Equal(lastNode, nodeToBeInserted.Right);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceAlongSublevelTo_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfInterveningNodes
            (List<INode<int>> nodes)
        {
	        // Fixture setup
            var sutStart = Substitute.For<INode<int>>();
            sutStart.Down = nodes.First();

            var sutEnd = Substitute.For<INode<int>>();
            sutEnd.Down = nodes.Last();

	        // Exercise system
            var result = sutStart.DistanceAlongSublevelTo(sutEnd);

            // Verify outcome
            Assert.Equal(nodes.Count-1, result);

            // Teardown
        }
    }
}
