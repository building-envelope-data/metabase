using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;

namespace Icon.GraphQl
{
    public class ComponentResolvers
      : ResolversBase
    {
        public ComponentResolvers(IQueryBus queryBus) : base(queryBus) { }

        public async Task<IEnumerable<ComponentVersion>> GetVersions(Component component, DateTime? timestamp)
        {
            return (await QueryBus.Send<
                  Queries.ListComponentVersions,
                  IEnumerable<Models.ComponentVersion>
                >(new Queries.ListComponentVersions(component.Id, timestamp)))
              .Select(c => ComponentVersion.FromModel(c));
        }
    }
}