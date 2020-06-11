using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

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
            [DataLoader] ComponentsManufacturedByInstitutionAssociationDataLoader manufacturedComponentsLoader
            )
        {
            return (await manufacturedComponentsLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                .ConfigureAwait(false)
                )
              .Select(a => new ManufacturedComponentEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class ComponentsManufacturedByInstitutionAssociationDataLoader
            : BackwardManyToManyAssociationsOfModelDataLoader<ComponentManufacturer, Models.Institution, Models.ComponentManufacturer>
        {
            public ComponentsManufacturedByInstitutionAssociationDataLoader(IQueryBus queryBus)
              : base(ComponentManufacturer.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetNodes(
            [DataLoader] ComponentsManufacturedByInstitutionDataLoader manufacturedComponentsLoader
            )
        {
            return manufacturedComponentsLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class ComponentsManufacturedByInstitutionDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.Institution, Models.ComponentManufacturer, Models.Component>
        {
            public ComponentsManufacturedByInstitutionDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}