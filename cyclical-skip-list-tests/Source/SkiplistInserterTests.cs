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
        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 2*MinimumGapSize;

        public SkiplistInserterTests()
        {
            SkiplistFactory.MinimumGapSize = MinimumGapSize;
        }

        [Theory]
        [FixedLengthSkiplistData(length: 0)]
        public void Insert_ingAKeyIntoAnEmptySkiplist_ShouldAddThatKeyAsTheHead
            (Skiplist<int> sut, int key)
        {
            // Fixture setup
            var expectedResult = key;

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            var result = sut.Head.Key;

            var failureString =
                String.Format(
                    "Inserting key {0} into skiplist \n\n{1}\n did not set the key as the head",
                    key,
                    sut);

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: MaximumGapSize)]
        public void Insert_ingAKeyIntoAMaximallySizedGap_ShouldSplitTheGap
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = MinimumGapSize + 1;

            var key = keys.Min();

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            var result = sut.Head.Down.SizeOfGap();

            var failureString =
                String.Format(
                    "Inserting key {0} into skiplist \n\n{1}\n did not split the first gap into two",
                    key,
                    sut);

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: MaximumGapSize*MaximumGapSize)]
        public void Insert_ingAKeyIntoASkiplistSuchThatTheTopLayerWouldHaveMoreThanASingleNode_ShouldIncreaseTheHeightByOne
            (Skiplist<int> sut, int key)
        {
            // Fixture setup
            var expectedResult = 3;

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            var result = sut.Head.Height();

            var failureString =
                String.Format(
                    "Inserting key {0} into skiplist \n\n{1}\n did not increment the height of the skiplist",
                    key,
                    sut);

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Insert_ingAKeyIntoASingleLayerSkiplist_ShouldAddThatKeyToTheSkiplist
            (Skiplist<int> sut, int key)
        {
            // Fixture setup

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            var result = sut.Head.Bottom().EnumerateRight().Any(node => node.Key == key);

            var failureString =
                String.Format(
                "Key {0} was inserted into skiplist \n\n{1}\n\n but was subsequently not found", 
                key, 
                sut);
            Assert.True(result, failureString);

            // Teardown
        }

        //[Theory]
        //[FixedLengthSkiplistData(length: 0)]
        //public void Insert_ingManyKeys_ShouldProduceAValidSkiplist
        //    (Skiplist<int> sut)
        //{
        //    var fixture = new Fixture();

        //    var keys = new List<int>();

        //    for (int i = 0; i < 40; i++)
        //    {
        //        var key = fixture.Create<int>();
        //        keys.Add(key);
        //        sut.Insert(key);
        //        Debug.WriteLine(sut);
        //    }

        //    var random = new Random();
        //    keys = keys.OrderBy(random.Next).ToList();

        //    foreach (int key in keys)
        //    {
        //        sut.Remove(key);
        //        Debug.WriteLine(sut);
        //    }

        //    Assert.True(true);
        //}
    }
}
