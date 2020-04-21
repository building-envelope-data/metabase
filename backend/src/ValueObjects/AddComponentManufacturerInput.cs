using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class AddComponentManufacturerInput
      : ValueObject
    {
        public Id ComponentId { get; }
        public Id InstitutionId { get; }
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        private AddComponentManufacturerInput(
            Id componentId,
            Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation
            )
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public static Result<AddComponentManufacturerInput, Errors> From(
            Id componentId,
            Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation
            )
        {
            return
              Result.Ok<AddComponentManufacturerInput, Errors>(
                  new AddComponentManufacturerInput(
                    componentId: componentId,
                    institutionId: institutionId,
                    marketingInformation: marketingInformation
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ComponentId;
            yield return InstitutionId;
            yield return MarketingInformation;
        }
    }
}