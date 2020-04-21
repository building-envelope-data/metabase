using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentManufacturer
      : NodeBase
    {
        public static ComponentManufacturer FromModel(
            Models.ComponentManufacturer model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new ComponentManufacturer(
                id: model.Id,
                componentId: model.ComponentId,
                institutionId: model.InstitutionId,
                marketingInformation: model.MarketingInformation is null ? null : ComponentManufacturerMarketingInformation.FromModel(model.MarketingInformation),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Guid ComponentId { get; }
        public Guid InstitutionId { get; }
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ComponentManufacturer(
            Guid id,
            Guid componentId,
            Guid institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation,
            DateTime timestamp,
            DateTime requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }
    }
}