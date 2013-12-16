using System.Collections.Generic;
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
        public void CreateFrom_GivenNoKeys_ShouldReturnNull( )
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
        public void CreateFrom_GivenAMaximumGapSizeNumberOfKeys_ShouldReturnSingleLayerSkiplistWithSameNumberOfElements
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
        [AutoSkiplistData(2, MaximumGapSize)]
        public void CreateFrom_Given2ToAMaximumGapSizeNumberOfKeys_ShouldReturnTwoLayerSkiplistWithOrderedBottomElements
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
        public void CreateFrom_GivenMoreThanTheMaximumGapSizeNumberOfKeys_ShouldReturnATopLevelWithGapsOfTheRightSize
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var currentNode = sut.Head;
            do
            {
                var gap = currentNode.SizeOfGapTo(currentNode.Right);
                Assert.True(gap >= MinimumGapSize);
                Assert.True(gap <= MaximumGapSize);

                currentNode = currentNode.Right;
            } while (currentNode != sut.Head);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void CreateFrom_WhenMakingATwoLevelSkiplist_ShouldReturnSkiplistWithASufficientNumberOfTopLevelNodes
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count / MaximumGapSize;

            // Exercise system

            // Verify outcome
            var result = sut.Head.Bottom().DistanceToSelf();
            Assert.True(result >= expectedResult);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void CreateFrom_WhenMakingATwoLevelSkiplist_ShouldCorrectlyAssignKeysToTheTopLevel
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            var expectedResult = SkiplistUtilities.EnumerateNodesInLevel(sut.Head).Select(node => node.Right.Down.Left().Key);
            var result = SkiplistUtilities.EnumerateKeysInLevel(sut.Head);
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(2, MaximumGapSize)]
        public void CreateFrom_ShouldAlwaysCreateASkiplistWithASingleNodeOnItsTopLevel
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
