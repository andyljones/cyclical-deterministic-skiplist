using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class NodeUtilitiesTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ConnectTo_OnTwoNonNullINodes_ShouldConnectThemHorizontally
            (INode<int> anonymousNodeA, INode<int> anonymousNodeB)
        {
            // Fixture setup

            // Exercise system
            anonymousNodeA.ConnectTo(anonymousNodeB);

            // Verify outcome
            Assert.Equal(anonymousNodeA, anonymousNodeB.Left);
            Assert.Equal(anonymousNodeB, anonymousNodeA.Right);

            // Teardown
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConnectDownTo_OnTwoNonNullINodes_ShouldConnectThemVertically
            (INode<int> anonymousNodeA, INode<int> anonymousNodeB)
        {
            // Fixture setup

            // Exercise system
            anonymousNodeA.ConnectDownTo(anonymousNodeB);

            // Verify outcome
            Assert.Equal(anonymousNodeA, anonymousNodeB.Up);
            Assert.Equal(anonymousNodeB, anonymousNodeA.Down);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void EnumerateRight_OnACircularListOfINodes_ShouldEncounterEveryKeyInTheList
            (Skiplist<int> list, List<int> keys)
        {
            // Fixture setup
            var expectedResults = keys.OrderBy(key => key);

            // Exercise system
            var nodesInList = list.Head.EnumerateRight();

            // Verify outcome
            var results = nodesInList.Select(node => node.Key);

            Assert.Equal(expectedResults, results);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void DistanceRightTo_OnTheHeadAndTailOfACircularListOfINodes_ShouldReturn1LessThanTheNumberOfNodesInTheList
            (Skiplist<int> list, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count - 1;

            // Exercise system
            var result = list.Head.DistanceRightTo(list.Head.Left);

            // Verify outcome
            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}