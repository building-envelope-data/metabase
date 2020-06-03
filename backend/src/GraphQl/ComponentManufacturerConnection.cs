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
            [DataLoader] ManufacturersOfComponentAssociationDataLoader manufacturersLoader
            )
        {
            return (await manufacturersLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                .ConfigureAwait(false)
                )
              .Select(a => new ComponentManufacturerEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class ManufacturersOfComponentAssociationDataLoader
            : ForwardManyToManyAssociationsOfModelDataLoader<ComponentManufacturer, Models.Component, Models.ComponentManufacturer>
        {
            public ManufacturersOfComponentAssociationDataLoader(IQueryBus queryBus)
              : base(ComponentManufacturer.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Institution>> GetNodes(
            [DataLoader] ManufacturersOfComponentDataLoader manufacturersLoader
            )
        {
            return manufacturersLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class ManufacturersOfComponentDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Institution, Models.Component, Models.ComponentManufacturer, Models.Institution>
        {
            public ManufacturersOfComponentDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}