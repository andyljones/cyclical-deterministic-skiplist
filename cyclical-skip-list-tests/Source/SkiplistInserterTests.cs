using System;
using System.Collections.Generic;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistInserterTests
    {
        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Insert_ingAKey_ShouldAddThatKeyToTheSkiplist
            (Skiplist<int> sut, int key)
        {
            // Fixture setup

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            var result = sut.Find(key);

            var failureString =
                String.Format(
                "Key {0} was inserted into skiplist \n\n{1}\n\n but was subsequently not found", 
                key, 
                sut);
            Assert.True(result, failureString);

            // Teardown
        }
    }
}
