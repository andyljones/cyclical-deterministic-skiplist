using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistValidatorTests
    {


        [Theory]
        [AutoSkiplistData(5, 10)]
        public void Test( )
        {
            // Fixture setup

            // Exercise system

            // Verify outcome
            Assert.True(false, "Test not implemented");

            // Teardown
        }
    }
}
