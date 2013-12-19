using System.Collections.Generic;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests.TestUtilities
{
    public class SkiplistFactoryTests
    {
        [Theory]
        [AutoData]
        public void CreateFrom_WhenGivenNoKeys_ShouldReturnASkiplistWithoutAHead
            ()
        {
            // Fixture setup
            var anonymousKeys = new List<int>(); 

            // Exercise system
            var sut = SkiplistFactory.CreateFrom(anonymousKeys);

            // Verify outcome
            Assert.True(sut.Head == null, "SUT has a head!");

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CreateFrom_WhenGivenAnyNonzeroNumberOfKeys_ShouldReturnASingleNodeTopLevel
            (IEnumerable<int> anonymousKeys)
        {
            // Fixture setup

            // Exercise system
            var sut = SkiplistFactory.CreateFrom(anonymousKeys);

            // Verify outcome
            Assert.True(sut.Head.Right == sut.Head, "Head does not connect to itself on the right!");
            Assert.True(sut.Head.Left == sut.Head, "Head does not connect to itself on the left!");

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CreateFrom_WhenGivenFewerKeysThanTheMaxGapSize_ShouldReturnADoublyLinkedList( )
        {
            // Fixture setup
            
            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
