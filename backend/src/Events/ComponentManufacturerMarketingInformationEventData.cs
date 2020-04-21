using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentManufacturerMarketingInformationEventData
      : Validatable
    {
        public static ComponentManufacturerMarketingInformationEventData From(
            ValueObjects.ComponentManufacturerMarketingInformation information
            )
        {
            return new ComponentManufacturerMarketingInformationEventData(
                componentInformation: information.ComponentInformation is null ? null : ComponentInformationEventData.From(information.ComponentInformation),
                institutionInformation: information.InstitutionInformation is null ? null : InstitutionInformationEventData.From(information.InstitutionInformation)
                );
        }

        public ComponentInformationEventData? ComponentInformation { get; set; }
        public InstitutionInformationEventData? InstitutionInformation { get; set; }

        public ComponentManufacturerMarketingInformationEventData() { }

        public ComponentManufacturerMarketingInformationEventData(
            ComponentInformationEventData? componentInformation,
            InstitutionInformationEventData? institutionInformation
            )
        {
            ComponentInformation = componentInformation;
            InstitutionInformation = institutionInformation;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  ComponentInformation?.Validate() ?? Result.Ok<bool, Errors>(true),
                  InstitutionInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }
    }
}