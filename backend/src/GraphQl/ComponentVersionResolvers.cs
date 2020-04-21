using Icon;
using CancellationToken = System.Threading.CancellationToken;
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
    [ExtendObjectType(Name = "ComponentVersion")]
    public class ComponentVersionResolvers
        : ResolversBase
    {
        public ComponentVersionResolvers(IQueryBus queryBus) : base(queryBus) { }

        public Task<Component[]> GetComponent(
            [Parent] ComponentVersion componentVersion,
            IResolverContext context
            )
        {
            return null!;
        }

        public Task<IEnumerable<Institution>> GetManufacturers(
            [Parent] ComponentVersion componentVersion,
            IResolverContext context
            )
        {
            return null!;
        }

        public Task<IEnumerable<ComponentVersion>> GetSuperComponentVersions(
            [Parent] ComponentVersion componentVersion,
            IResolverContext context
            )
        {
            return null!;
        }

        public Task<IEnumerable<ComponentVersion>> GetSubComponentVersions(
            [Parent] ComponentVersion componentVersion,
            IResolverContext context
            )
        {
            return null!;
        }
    }
}