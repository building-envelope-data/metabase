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
    public sealed class ComponentManufacturerEdge
      : Edge
    {
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ComponentManufacturerEdge(
            ComponentManufacturer componentManufacturer
            )
          : base(
              nodeId: componentManufacturer.InstitutionId,
              requestTimestamp: componentManufacturer.RequestTimestamp
              )
        {
            MarketingInformation = componentManufacturer.MarketingInformation;
        }

        public Task<Institution> GetNode(
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(NodeId, RequestTimestamp)
                );
        }
    }
}