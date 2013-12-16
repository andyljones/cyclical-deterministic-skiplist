using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistFactoryTests
    {
        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

        [Fact]
        public void SkiplistFactory_GivenNoKeys_ShouldReturnNull( )
        {
            // Fixture setup

            // Exercise system
            var result = SkiplistFactory.CreateFrom(new List<int>());

            // Verify outcome
            Assert.Equal(null, result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1, MaximumGapSize)]
        public void SkiplistFactory_GivenAMaximumGapSizeNumberOfKeys_ShouldReturnTwoLayerSkiplistWithSameNumberOfElements
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count;

            // Exercise system

            // Verify outcome
            var result = sut.Head.Bottom().DistanceToSelf();
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1, MaximumGapSize)]
        public void SkiplistFactory_GivenAMaximumGapSizeNumberOfKeys_ShouldReturnTwoLayerSkiplistWithBottomListCorrectlyConnected
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var nodes = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Bottom());
            Assert.True(nodes.All(node => node.Right.Left == node));

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(2, MaximumGapSize)]
        public void SkiplistFactory_Given2ToAMaximumGapSizeNumberOfKeys_ShouldReturnTwoLayerSkiplistWithOrderedBottomElements
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResults = keys.OrderBy(key => key);

            // Exercise system

            // Verify outcome
            var results = SkiplistUtilities.EnumerateKeysInLevel(sut.Head.Bottom());
            Assert.Equal(expectedResults, results);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void SkiplistFactory_GivenMoreThanTheMaximumGapSizeNumberOfKeys_ShouldReturnAMidLevelWithGapsOfTheRightSize
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var currentNode = sut.Head.Down;
            do
            {
                var gap = currentNode.SizeOfGapTo(currentNode.Right);
                Assert.True(gap >= MinimumGapSize);
                Assert.True(gap <= MaximumGapSize);

                currentNode = currentNode.Right;
            } while (currentNode != sut.Head.Down);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize + 1, 3 * MaximumGapSize)]
        public void SkiplistFactory_GivenMoreThanTheMaximumGapSizeNumberOfKeys_ShouldReturnTwoLayerSkiplistWithMidLevelCorrectlyConnected
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var nodes = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Down);
            Assert.True(nodes.All(node => node.Right.Left == node));

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void SkiplistFactory_WhenMakingAThreeLevelSkiplist_ShouldReturnSkiplistWithASufficientNumberOfMidLevelNodes
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count / MaximumGapSize;

            // Exercise system

            // Verify outcome
            var result = sut.Head.Down.DistanceToSelf();
            Assert.True(result >= expectedResult);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void SkiplistFactory_WhenMakingAThreeLevelSkiplist_ShouldCorrectlyAssignKeysToTheMidLevel
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup

            // Exercise system
            Debug.WriteLine(sut.ToString());

            // Verify outcome
            var expectedResult = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Down).Select(node => node.Right.Down.Left().Key);
            var result = SkiplistUtilities.EnumerateKeysInLevel(sut.Head.Down);
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(2, MaximumGapSize)]
        public void SkiplistFactory_ShouldAlwaysCreateASkiplistWithASingleNodeOnItsTopLevel
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = 1;

            // Exercise system

            // Verify outcome
            var result = SkiplistUtilities.EnumerateNodesInLevel(sut.Head).Count();
            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
