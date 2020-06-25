using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class ComponentManufacturer
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public ValueObjects.Id ParentId { get => ComponentId; }
        public ValueObjects.Id AssociateId { get => InstitutionId; }

        private ComponentManufacturer(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public static Result<ComponentManufacturer, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Timestamp timestamp
            )
        {
            return
                Result.Ok<ComponentManufacturer, Errors>(
                    new ComponentManufacturer(
                        id: id,
                        componentId: componentId,
                        institutionId: institutionId,
                        marketingInformation: marketingInformation,
                        timestamp: timestamp
                        )
                    );
        }
    }
}