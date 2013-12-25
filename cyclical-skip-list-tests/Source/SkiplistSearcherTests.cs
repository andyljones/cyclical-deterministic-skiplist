using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistSearcherTests
    {
        [Theory]
        [FixedHeightSkiplistData(3)]
        public void Find_GivenAKeyThatIsInTheSkiplist_ShouldReturnTrue
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var key = keys.First();

            // Exercise system
            var result = sut.Find(key);

            // Verify outcome
            var failureString =
                String.Format(
                "Failed to find key {0} in skiplist \n\n {1}",
                key, 
                sut);
            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(3)]
        public void Find_GivenAKeyThatIsNotInTheSkiplist_ShouldReturnFalse
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var missingKeys = Enumerable.Range(keys.Min(), keys.Max()).Except(keys).ToList();
            var missingKey = missingKeys[new Random().Next(missingKeys.Count())];

            // Exercise system
            var result = sut.Find(missingKey);

            // Verify outcome
            var failureString =
                String.Format(
                "Found key {0} that should be missing from skiplist \n\n {1}",
                missingKey,
                sut);
            Assert.False(result, failureString);

            // Teardown
        }
    }
}
