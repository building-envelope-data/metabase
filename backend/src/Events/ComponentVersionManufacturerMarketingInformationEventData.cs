using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerMarketingInformationEventData
      : Validatable
    {
        public static ComponentVersionManufacturerMarketingInformationEventData From(
            ValueObjects.ComponentVersionManufacturerMarketingInformation information
            )
        {
            return new ComponentVersionManufacturerMarketingInformationEventData(
                componentVersionInformation: information.ComponentVersionInformation is null ? null : ComponentInformationEventData.From(information.ComponentVersionInformation),
                institutionInformation: information.InstitutionInformation is null ? null : InstitutionInformationEventData.From(information.InstitutionInformation)
                );
        }

        public ComponentInformationEventData? ComponentVersionInformation { get; set; }
        public InstitutionInformationEventData? InstitutionInformation { get; set; }

        public ComponentVersionManufacturerMarketingInformationEventData() { }

        public ComponentVersionManufacturerMarketingInformationEventData(
            ComponentInformationEventData? componentVersionInformation,
            InstitutionInformationEventData? institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
          return
            Result.Combine(
                ComponentVersionInformation?.Validate() ?? Result.Ok<bool, Errors>(true),
                InstitutionInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                );
        }
    }
}