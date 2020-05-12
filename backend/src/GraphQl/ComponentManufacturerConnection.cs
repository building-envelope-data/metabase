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
            : ForwardAssociationsOfModelDataLoader<ComponentManufacturer, Models.Component, Models.ComponentManufacturer>
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
            : ForwardAssociatesOfModelDataLoader<Institution, Models.Component, Models.ComponentManufacturer, Models.Institution>
        {
            public ManufacturersOfComponentDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}