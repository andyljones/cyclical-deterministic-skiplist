using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class FactoryTests
    {
        private const int MinimumGapSize = 1;
        private const int MaximumGapSize = 4;

        [Fact]
        public void CreateFrom_GivenNoKeys_ShouldReturnNull( )
        {
            // Fixture setup

            // Exercise system
            var result = Factory.CreateFrom(new List<int>());

            // Verify outcome
            Assert.Equal(null, result);

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(1, MaximumGapSize)]
        public void CreateFrom_GivenAMaximumGapSizeNumberOfKeys_ShouldReturnSingleLayerSkiplistWithSameNumberOfElements
            (List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count;

            // Exercise system
            var sutHead = Factory.CreateFrom(keys);

            // Verify outcome
            var result = sutHead.DistanceToSelf();
            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(2, MaximumGapSize)]
        public void CreateFrom_Given2ToAMaximumGapSizeNumberOfKeys_ShouldReturnSingleLayerSkiplistWithOrderedElements
            (List<int> keys)
        {
            // Fixture setup
            var expectedResults = keys.OrderBy(key => key);

            // Exercise system
            var sutHead = Factory.CreateFrom(keys);

            // Verify outcome
            var results = Utilities.EnumerateKeysInLevel(sutHead);
            Assert.Equal(expectedResults, results);

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void CreateFrom_GivenMoreThanTheMaximumGapSizeNumberOfKeys_ShouldReturnATopLevelWithGapsOfTheRightSize
            (List<int> keys)
        {
            // Fixture setup

            // Exercise system
            var sutHead = Factory.CreateFrom(keys);

            // Verify outcome
            var currentNode = sutHead;
            do
            {
                var gap = currentNode.SizeOfGapTo(currentNode.Right);
                Assert.True(gap >= MinimumGapSize);
                Assert.True(gap <= MaximumGapSize);

                currentNode = currentNode.Right;
            } while (currentNode != sutHead);

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void CreateFrom_WhenMakingATwoLevelSkiplist_ShouldReturnSkiplistWithASufficientNumberOfTopLevelNodes
            (List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count / MaximumGapSize;

            // Exercise system
            var sutHead = Factory.CreateFrom(keys);

            // Verify outcome
            var result = sutHead.DistanceToSelf();
            Assert.True(result >= expectedResult);

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(MaximumGapSize+1, 3*MaximumGapSize)]
        public void CreateFrom_WhenMakingATwoLevelSkiplist_ShouldCorrectlyAssignKeysToTheTopLevel
            (List<int> keys)
        {
            // Fixture setup

            // Exercise system
            var sutHead = Factory.CreateFrom(keys);

            // Verify outcome
            var expectedResult = Utilities.EnumerateNodesInLevel(sutHead).Select(node => node.Right.Down.Left().Key);            
            var result = Utilities.EnumerateKeysInLevel(sutHead);
            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
