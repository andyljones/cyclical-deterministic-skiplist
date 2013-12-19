using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class NodeUtilitiesTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ConnectTo_OnTwoNonNullINodes_ShouldConnectThemHorizontally
            (INode<int> anonymousNodeA, INode<int> anonymousNodeB)
        {
            // Fixture setup

            // Exercise system
            anonymousNodeA.ConnectTo(anonymousNodeB);

            // Verify outcome
            Assert.Equal(anonymousNodeA, anonymousNodeB.Left);
            Assert.Equal(anonymousNodeB, anonymousNodeA.Right);

            // Teardown
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConnectDownTo_OnTwoNonNullINodes_ShouldConnectThemVertically
            (INode<int> anonymousNodeA, INode<int> anonymousNodeB)
        {
            // Fixture setup

            // Exercise system
            anonymousNodeA.ConnectDownTo(anonymousNodeB);

            // Verify outcome
            Assert.Equal(anonymousNodeA, anonymousNodeB.Up);
            Assert.Equal(anonymousNodeB, anonymousNodeA.Down);

            // Teardown
        }
    }
}