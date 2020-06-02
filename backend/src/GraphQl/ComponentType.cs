using Models = Icon.Models;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Language;
using HotChocolate.Types;

namespace Icon.GraphQl
{
    public sealed class ComponentType
      : ObjectType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            descriptor
              .Field(f => f.GetOpticalData(null!))
              .Type<NonNullType<ListType<NonNullType<AnyType>>>>();
        }
    }
}