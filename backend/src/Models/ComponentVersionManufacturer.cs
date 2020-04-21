using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersionManufacturer
      : Model
    {
        public ValueObjects.Id ComponentVersionId { get; }
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        private ComponentVersionManufacturer(
            ValueObjects.Id id,
            ValueObjects.Id componentVersionId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public static Result<ComponentVersionManufacturer, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentVersionId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentVersionManufacturer, Errors>(
                  new ComponentVersionManufacturer(
                    id: id,
                    componentVersionId: componentVersionId,
                    institutionId: institutionId,
                    marketingInformation: marketingInformation,
                    timestamp: timestamp
                    )
                  );
        }
    }
}