using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests.TestUtilities
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute()
        {
            Fixture.Customize(new AutoNSubstituteCustomization());
        }
    }
}
