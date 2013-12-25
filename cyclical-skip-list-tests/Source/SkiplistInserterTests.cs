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

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Insert_ingAKey_ShouldPreserveTheOrdering
            (Skiplist<int> sut, int key)
        {
            // Fixture setup

            // Exercise system
            sut.Insert(key);

            // Verify outcome
            IEnumerable<INode<int>> failingNodes;
            var result = SkiplistValidator.ValidateOrdering(sut, out failingNodes);

            var failureString =
                String.Format(
                    "Key {0} was inserted into skiplist \n\n{1}\n\n and subsequently nodes {2} were found to be out of order",
                    key,
                    sut,
                    String.Join(", ", failingNodes));
            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 2)]
        public void Insert_ingManyKeys_ShouldPreserveTheMaximumGapSize
            (Skiplist<int> sut, int key)
        {
            // Fixture setup

            // Exercise system
            for (int i = 0; i < sut.MaximumGapSize + 1; i++)
            {
                sut.Insert(key);
            }

            // Verify outcome
            IEnumerable<INode<int>> failingNodes;
            var result = SkiplistValidator.ValidateGapSize(sut, out failingNodes);

            var failureString =
                String.Format(
                    "Key {0} was inserted {1} times into skiplist \n\n{2}\n and subsequently nodes {3} were found to have invalid gap sizes",
                    key,
                    sut.MaximumGapSize,
                    sut,
                    String.Join(", ", failingNodes));
            Assert.True(result, failureString);

            // Teardown
        }
    }
}
