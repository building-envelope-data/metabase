using Models = Icon.Models;
using System.Linq;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Icon.GraphQl
{
    public sealed class ComponentManufacturerConnection
      : Connection
    {
        public ComponentManufacturerConnection(
            Component component
            )
          : base(
              fromId: component.Id,
              pageInfo: null!,
              requestTimestamp: component.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<ComponentManufacturerEdge>> GetEdges(
            [DataLoader] ManufacturersOfComponentIdentifiedByTimestampedIdAssociationDataLoader manufacturersLoader
            )
        {
            return (await manufacturersLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                )
              .Select(a => new ComponentManufacturerEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class ManufacturersOfComponentIdentifiedByTimestampedIdAssociationDataLoader
            : ForwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader<ComponentManufacturer, Models.Component, Models.ComponentManufacturer>
        {
            public ManufacturersOfComponentIdentifiedByTimestampedIdAssociationDataLoader(IQueryBus queryBus)
              : base(ComponentManufacturer.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Institution>> GetNodes(
            [DataLoader] ManufacturersOfComponentIdentifiedByTimestampedIdDataLoader manufacturersLoader
            )
        {
            return manufacturersLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class ManufacturersOfComponentIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Institution, Models.Component, Models.ComponentManufacturer, Models.Institution>
        {
            public ManufacturersOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}