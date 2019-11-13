using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;
using HotChocolate.Resolvers;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public class ComponentResolvers
      : ResolversBase
    {
        public ComponentResolvers(IQueryBus queryBus) : base(queryBus) { }

        public async Task<IEnumerable<ComponentVersion>> GetVersions(
            [Parent] Component component,
            IResolverContext context
            )
        {
            return ResultHelpers.HandleFailures<Models.ComponentVersion>(
                await QueryBus.Send<
                  Queries.ListComponentVersions,
                  IEnumerable<Result<Models.ComponentVersion, IError>>
                >(new Queries.ListComponentVersions(component.Id, Timestamp.Fetch(context)))
                )
              .Select(c => ComponentVersion.FromModel(c));
        }
    }
}