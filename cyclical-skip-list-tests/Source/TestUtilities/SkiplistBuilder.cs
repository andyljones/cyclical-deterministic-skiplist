using System.Collections.Generic;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace CyclicalSkipListTests.TestUtilities
{
    public class SkiplistBuilder<T> : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var skiplist = request as Skiplist<T>;
            if (skiplist == null)
            {
                return new NoSpecimen(request);
            }
            else
            {
                return SkiplistFactory.CreateFrom(context.Create<List<T>>());
            }
        }
    }
}
