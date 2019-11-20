using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerMarketingInformationEventData
      : Validatable
    {
        public static ComponentVersionManufacturerMarketingInformationEventData From(
            Models.ComponentVersionManufacturerMarketingInformation information
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

        public override bool IsValid()
        {
            return (ComponentVersionInformation?.IsValid() ?? true) &&
              (InstitutionInformation?.IsValid() ?? true);
        }
    }
}