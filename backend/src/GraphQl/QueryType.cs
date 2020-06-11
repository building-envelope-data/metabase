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
    public sealed class QueryType
      : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            /* descriptor */
            /*   .Field(f => f.GetOpticalData(null!, null!, null!, null!)) */
            /*   .Type<NonNullType<ListType<NonNullType<AnyType>>>>(); */
        }
    }
}