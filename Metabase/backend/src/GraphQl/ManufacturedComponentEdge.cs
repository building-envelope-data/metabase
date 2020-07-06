using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
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