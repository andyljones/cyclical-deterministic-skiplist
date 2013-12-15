using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using NSubstitute;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Kernel;
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

    public class AutoRandomRepeatCountDataAttribute : AutoDataAttribute
    {
        public AutoRandomRepeatCountDataAttribute(int lowerBound, int upperBound)
            : base(new Fixture().Customize(new RandomRepeatCountCustomization(lowerBound, upperBound)))
        {   
        }
    }

    public class AutoIsolatedNodeData : AutoDataAttribute
    {
        public AutoIsolatedNodeData()
            : base(new Fixture().Customize(new IsolatedNodeCustomization<int>()))
        {
        }

        public AutoIsolatedNodeData(int lowerBound, int upperBound)
            : base(new Fixture().Customize(new IsolatedNodeCustomization<int>())
                                .Customize(new RandomRepeatCountCustomization(lowerBound, upperBound)))
        {
        }
    }

    public class AutoINodeLinkedListData : AutoDataAttribute
    {
        public AutoINodeLinkedListData(int lowerBound, int upperBound)
            : base(CustomizedFixture<int>(lowerBound, upperBound))
        {
        }

        private static IFixture CustomizedFixture<T>(int lowerBound, int upperBound)
        {
            var fixture = new Fixture().Customize(new RandomRepeatCountCustomization(lowerBound, upperBound));
            fixture.Freeze<List<int>>();

            var nodes = CreateCircularLinkedList<T>(fixture);

            fixture.Inject(nodes);
            fixture.Inject(nodes[0]);

            return fixture;
        }

        private static List<INode<T>> CreateCircularLinkedList<T>(IFixture fixture)
        {
            var keys = fixture.Create<List<T>>();
            var nodes = keys.Select(i => { var node = Substitute.For<INode<T>>();
                                             node.Key = i;
                                             node.Down = null;
                                             return node;
                                         }).ToList();

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                nodes[i].Right = nodes[i + 1];
            }
            nodes[nodes.Count - 1].Right = nodes[0];
            
            return nodes;
        }
    }

    public class AutoSkiplistData : AutoDataAttribute
    {
        public AutoSkiplistData(int lowerBound, int upperBound)
            : base(CustomizedFixture(lowerBound, upperBound))
        {
        }

        private static IFixture CustomizedFixture(int lowerBound, int upperBound)
        {
            var fixture = new Fixture().Customize(new RandomRepeatCountCustomization(lowerBound, upperBound))
                                       .Customize(new DistinctIntCustomization());

            fixture.Inject(SkiplistFactory.CreateFrom(fixture.Create<List<int>>()));

            return fixture;
        }
    }
}
