using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistRemoverTests
    {
        [Theory]
        [AutoSkiplistData(height: 1)]
        public void Remove_ingAKeyWhichAppearsOnceInASkiplist_ShouldDeleteThatKeyFromASingleLayerSkiplist
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var key = keys.First();

            // Exercise system
            sut.Remove(key);

            // Verify outcome
            var result = sut.Find(key);

            var failureString =
                String.Format(
                "Key {0} was removed from skiplist \n\n{1}\n\n but was subsequently found anyway", 
                key, 
                sut);
            Assert.False(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 2)]
        public void Remove_ingManyKeys_ShouldPreserveTheMinimumGapSize
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var keysToBeRemoved = keys.OrderBy(x => x).Take(2*sut.MaximumGapSize).ToList();

            // Exercise system
            foreach (var key in keysToBeRemoved)
            {
                sut.Remove(key);
            }

            // Verify outcome
            IEnumerable<INode<int>> failingNodes;
            var result = SkiplistValidator.ValidateGapSize(sut, out failingNodes);

            var failureString =
                String.Format(
                    "Keys {0} were removed from skiplist \n\n{1}\n and subsequently nodes {2} were found to have invalid gap sizes",
                    String.Join(", ", keysToBeRemoved),
                    sut,
                    String.Join(", ", failingNodes));
            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void Remove_ingAllKeys_ShouldLeaveASkiplistWithAnEmptyHead
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup

            // Exercise system
            foreach (var key in keys)
            {
                sut.Remove(key);
            }

            // Verify outcome
            var result = sut.Head == null;

            var failureString =
                String.Format(
                    "All keys were to be removed from skiplist \n\n{0}\n, but a head remains",
                    sut);
            Assert.True(result, failureString);

            // Teardown
        }
    }
}
