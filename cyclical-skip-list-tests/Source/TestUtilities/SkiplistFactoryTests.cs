using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests.TestUtilities
{
    public class SkiplistFactoryTests
    {
        [Theory]
        [AutoSkiplistData(0)]
        public void CreateFrom_WhenGivenNoKeys_ShouldReturnASkiplistWithoutAHead
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            Assert.True(sut.Head == null, "SUT has a head!");

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1)]
        public void CreateFrom_WhenGivenFewerKeysThanTheMaxGapSize_ShouldReturnADoublyLinkedList
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var nodes = sut.Head.EnumerateRight().ToList();
            Assert.False(nodes.Any(node => node.Right.Left != node), "There are nodes for which a right-left move does not return the origin!");
            Assert.False(nodes.Any(node => node.Left.Right != node), "There are nodes for which a left-right move does not return the origin!");

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1)]
        public void CreateFrom_WhenGivenFewerKeysThanTheMaxGapSize_ShouldReturnAListOfNodesContainingAllTheKeysInOrder
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.OrderBy(key => key);

            // Exercise system

            // Verify outcome
            var result = sut.Head.EnumerateRight().Select(node => node.Key);

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(3)]
        public void CreateFrom_WhenGivenMoreKeysThanTheMaxGapsize_ShouldReturnASkiplistWithATopLevelWithFewerNodesThanTheMaxGapsize
            (Skiplist<int> sut)
        {
            // Fixture setup
            var expectedResult = SkiplistFactory.MaximumGapSize;

            // Exercise system

            // Verify outcome
            var result = sut.Head.LengthOfList();

            Assert.True(expectedResult >= result, "There are more nodes in the top level than the maximum gap size!");

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(3)]
        public void CreateFrom_WhenGivenMoreKeysThanTheMaxGapsize_ShouldReturnASkiplistWithEveryUpperLevelWithFewerNodesInTheGapThanTheMaxGapsize
            (Skiplist<int> sut)
        {
            // Fixture setup
            var expectedResult = SkiplistFactory.MaximumGapSize;

            // Exercise system
            Debug.WriteLine(sut);

            // Verify outcome
            var upperLevels = sut.Head.EnumerateDown().TakeWhile(level => level.Down != null);
            var upperLevelNodes = upperLevels.SelectMany(level => level.EnumerateRight());
            var results = upperLevelNodes.Select(node => node.SizeOfGap());

            Assert.True(results.All(result => expectedResult >= result), "There are more nodes in the top level than the maximum gap size!");

            // Teardown
        }
    }
}
