using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

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
              timestamp: componentManufacturer.Timestamp,
              requestTimestamp: componentManufacturer.RequestTimestamp
              )
        {
            MarketingInformation = componentManufacturer.MarketingInformation;
        }

        public Task<Institution> GetNode(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(NodeId, RequestTimestamp)
                );
        }
    }
}