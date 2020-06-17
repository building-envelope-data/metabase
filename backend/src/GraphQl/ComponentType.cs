using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentType
      : ObjectType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            /* descriptor */
            /*   .Field(f => f.GetOpticalData(null!, null!)) */
            /*   .Type<NonNullType<ListType<NonNullType<AnyType>>>>(); */
        }
    }
}