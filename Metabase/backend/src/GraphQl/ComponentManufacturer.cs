using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class ComponentManufacturer
      : Node
    {
        public static ComponentManufacturer FromModel(
            Models.ComponentManufacturer model,
            Timestamp requestTimestamp
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

        public Id ComponentId { get; }
        public Id InstitutionId { get; }
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ComponentManufacturer(
            Id id,
            Id componentId,
            Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation,
            Timestamp timestamp,
            Timestamp requestTimestamp
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