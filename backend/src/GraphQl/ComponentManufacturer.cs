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

        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id InstitutionId { get; }
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ComponentManufacturer(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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