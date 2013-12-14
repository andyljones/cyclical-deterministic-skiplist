using System.Collections.Generic;
using CyclicalSkipList;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistFactoryTests
    {
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
        [AutoRandomRepeatCountData(1, 3)]
        public void CreateFrom_Given1To3Keys_ShouldReturnSingleLayerSkiplistWithSameNumberOfElements
            (List<int> keys)
        {
            // Fixture setup

            // Exercise system
            var result = SkiplistFactory.CreateFrom(keys);

            // Verify outcome
            Assert.Equal(keys.Count, result.Count());

            // Teardown
        }

        [Theory]
        [AutoRandomRepeatCountData(2, 3)]
        public void CreateFrom_Given2To3Keys_ShouldReturnSingleLayerSkiplistWithOrderedElements
            (List<int> keys)
        {
            // Fixture setup

            // Exercise system
            var result = SkiplistFactory.CreateFrom(keys);

            // Verify outcome
            var position = 1;

            var currentNode = result;
            while (position < keys.Count)
            {
                Assert.True(currentNode.Right.Key > currentNode.Key);
                position++;
                currentNode = currentNode.Right;
            }

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CreateFrom_Given4To12Keys_ShouldReturnATopLevelWith( )
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
