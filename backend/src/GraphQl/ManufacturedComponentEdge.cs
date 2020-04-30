using Models = Icon.Models;
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
    public sealed class ManufacturedComponentEdge
      : Edge
    {
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ManufacturedComponentEdge(
            ComponentManufacturer componentManufacturer
            )
          : base(
              nodeId: componentManufacturer.ComponentId,
              requestTimestamp: componentManufacturer.RequestTimestamp
              )
        {
            MarketingInformation = componentManufacturer.MarketingInformation;
        }

        public Task<Component> GetNode(
            [DataLoader] ComponentForTimestampedIdDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(NodeId, RequestTimestamp)
                );
        }
    }
}