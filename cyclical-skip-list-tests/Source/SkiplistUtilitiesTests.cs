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
    public class SkiplistUtilitiesTests
    {
        private const int LowerLengthBound = 3;
        private const int UpperLengthBound = 10;

        [Theory]
        [AutoIsolatedNodeData(LowerLengthBound, UpperLengthBound)]
        public void Bottom_GivenAVerticalLinkedList_ReturnsTheBottomNode
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

            var listHead = nodes.Last();

            // Exercise system
            var result = listHead.Bottom();

            // Verify outcome
            Assert.Equal(nodes[0], result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceTo_GivenAHorizontalLinkedList_ReturnsTheDistanceBetweenTheSpecifiedNodes
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = listHead.DistanceTo(nodes[0]);
            var result1 = listHead.DistanceTo(nodes[1 % nodes.Count]);
            var result2 = listHead.DistanceTo(nodes[nodes.Count - 1]);

            // Verify outcome
            Assert.Equal(0 % nodes.Count, result0);
            Assert.Equal(1 % nodes.Count, result1);
            Assert.Equal(nodes.Count - 1, result2);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceToSelf_GivenAHorizontalLinkedList_ReturnsTheLengthOfTheList
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = listHead.DistanceToSelf();

            // Verify outcome
            Assert.Equal(nodes.Count, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void MoveRightBy_GivenAHorizontalLinkedList_ReturnsANodeTheCorrectDistanceToTheRightOfSpecifiedNode
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = listHead.RightBy(0);
            var result1 = listHead.RightBy(2);
            var result2 = listHead.RightBy(nodes.Count + 1);

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
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void SizeOfGapTo_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfInterveningNodes
            (List<INode<int>> nodes)
        {
	        // Fixture setup
            var start = Substitute.For<INode<int>>();
            start.Down = nodes.First();

            var end = Substitute.For<INode<int>>();
            end.Down = nodes.Last();

	        // Exercise system
            var result = start.SizeOfGapTo(end);

            // Verify outcome
            Assert.Equal(nodes.Count-1, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void SizeOfGap_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfNodesBetweenTheNodeAndItsRightNeighbour
            (List<INode<int>> nodes)
        {
            // Fixture setup
            var start = Substitute.For<INode<int>>();
            start.Down = nodes.First();

            var end = Substitute.For<INode<int>>();
            end.Down = nodes.Last();

            start.ConnectTo(end);

            // Exercise system
            var result = start.SizeOfGap();

            // Verify outcome
            Assert.Equal(nodes.Count - 1, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void SizeOfBaseGapTo_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfInterveningNodesAtBottomLevel
            (List<INode<int>> nodes)
        {
            // Fixture setup
            var start = Substitute.For<INode<int>>();
            var midlevelStart = Substitute.For<INode<int>>();
            start.Down = midlevelStart;
            midlevelStart.Down = nodes.First();

            var end = Substitute.For<INode<int>>();
            var midlevelEnd = Substitute.For<INode<int>>();
            end.Down = midlevelEnd;
            midlevelEnd.Down = nodes.Last();

            // Exercise system
            var result = start.SizeOfBaseGapTo(end);

            // Verify outcome
            Assert.Equal(nodes.Count - 1, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateKeysInLevel_GivenALinkedList_ShouldEnumerateKeysInThatLevel
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = SkiplistUtilities.EnumerateKeysInLevel(listHead);

            // Verify outcome
            var expectedResult = nodes.Select(node => node.Key);

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateNodesInLevel_GivenALinkedList_ShouldEnumerateNodesInThatLevel
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = SkiplistUtilities.EnumerateNodesInLevel(listHead);

            // Verify outcome
            var expectedResult = nodes;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateLevels_GivenAVerticalLinkedList_EnumeratesEachNodeBelowTheNodeSpecified
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

            var listHead = nodes.Last();
            
            // Exercise system
            var result = SkiplistUtilities.EnumerateLevels(listHead).ToList();

            // Verify outcome
            result.Reverse();
            Assert.Equal(nodes, result);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData]
        public void ConnectTo_OnTwoNodes_ShouldSetTheirPointersCorrectly
            (INode<int> leftNode, INode<int> rightNode)
        {
            // Fixture setup

            // Exercise system
            leftNode.ConnectTo(rightNode);

            // Verify outcome
            Assert.Equal(rightNode, leftNode.Right);
            Assert.Equal(leftNode, rightNode.Left);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData]
        public void DisconnectLeft_OnTwoNodes_ShouldSetTheirRelevantPointersToNull
            (INode<int> leftNode, INode<int> rightNode)
        {
            // Fixture setup
            leftNode.ConnectTo(rightNode);

            // Exercise system
            rightNode.DisconnectLeft();

            // Verify outcome
            Assert.Equal(null, leftNode.Right);
            Assert.Equal(null, rightNode.Left);

            // Teardown
        }


        [Theory]
        [AutoIsolatedNodeData]
        public void DisconnectRight_OnTwoNodes_ShouldSetTheirRelevantPointersToNull
            (INode<int> leftNode, INode<int> rightNode)
        {
            // Fixture setup
            leftNode.ConnectTo(rightNode);

            // Exercise system
            leftNode.DisconnectRight();

            // Verify outcome
            Assert.Equal(null, rightNode.Left);
            Assert.Equal(null, leftNode.Right);

            // Teardown
        }
    }
}
