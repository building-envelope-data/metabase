using System.Collections.Generic;
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
    public sealed class ManufacturedComponentEdge
      : Edge
    {
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ManufacturedComponentEdge(
            ComponentManufacturer componentManufacturer
            )
          : base(
              nodeId: componentManufacturer.ComponentId,
              timestamp: componentManufacturer.Timestamp,
              requestTimestamp: componentManufacturer.RequestTimestamp
              )
        {
            MarketingInformation = componentManufacturer.MarketingInformation;
        }

        public Task<Component> GetNode(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(NodeId, RequestTimestamp)
                );
        }
    }
}