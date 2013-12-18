using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistSearcherTests
    {
        private const int MinimumLength = 10;
        private const int MaximumLength = 20;

        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistContainsTheGivenKey_ShouldReturnTrue
            (Skiplist<int> sut, IList<int> keys)
        {
            // Fixture setup
            var key = keys[new Random().Next(0, keys.Count - 1)];

            // Exercise system
            var result = sut.Contains(key);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistDoesNotContainAKeyWhichIsWithinTheRangeOfItsKeys_ShouldReturnFalse
            (Skiplist<int> sut, IList<int> keys, int distinctKey)
        {
            // Fixture setup

            // Exercise system
            var result = sut.Contains(distinctKey);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistDoesNotContainAKeyLargerThanItsKeys_ShouldReturnFalse
            (Skiplist<int> sut, IList<int> keys)
        {
            // Fixture setup
            var key = keys.Max() + 1;

            // Exercise system
            var result = sut.Contains(key);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }
    }
}
