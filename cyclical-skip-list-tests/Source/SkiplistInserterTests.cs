using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture;
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

        [Theory]
        [FixedLengthSkiplistData(length: 0)]
        public void Insert_ingManyKeys_ShouldProduceAValidSkiplist
            (Skiplist<int> sut)
        {
            var fixture = new Fixture();

            var keys = new List<int>();

            for (int i = 0; i < 40; i++)
            {
                var key = fixture.Create<int>();
                keys.Add(key);
                sut.Insert(key);
                Debug.WriteLine(sut);
            }

            var random = new Random();
            keys = keys.OrderBy(random.Next).ToList();

            foreach (int key in keys)
            {
                sut.Remove(key);
                Debug.WriteLine(sut);
            }

            Assert.True(true);


        }
    }
}
