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
    public sealed class ManufacturedComponentConnection
      : Connection
    {
        public ManufacturedComponentConnection(
            Institution institution
            )
          : base(
              fromId: institution.Id,
              pageInfo: null!,
              requestTimestamp: institution.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<ManufacturedComponentEdge>> GetEdges(
            [DataLoader] ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdAssociationDataLoader manufacturedComponentsLoader
            )
        {
            return (await manufacturedComponentsLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                )
              .Select(a => new ManufacturedComponentEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdAssociationDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<ComponentManufacturer, Models.Institution, Models.ComponentManufacturer>
        {
            public ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdAssociationDataLoader(IQueryBus queryBus)
              : base(ComponentManufacturer.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetNodes(
            [DataLoader] ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader manufacturedComponentsLoader
            )
        {
            return manufacturedComponentsLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Institution, Models.Component>
        {
            public ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}