using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests
{
    public class AutoNSubDataAttribute : AutoDataAttribute
    {
        public AutoNSubDataAttribute()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
