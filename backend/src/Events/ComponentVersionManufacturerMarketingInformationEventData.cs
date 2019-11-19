using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerMarketingInformationEventData
    {
        public static ComponentVersionManufacturerMarketingInformationEventData From(
            Models.ComponentVersionManufacturerMarketingInformation information
            )
        {
            return new ComponentVersionManufacturerMarketingInformationEventData(
                componentVersionInformation: ComponentInformationEventData.From(information.ComponentVersionInformation),
                institutionInformation: InstitutionInformationEventData.From(information.InstitutionInformation)
                );
        }

        public ComponentInformationEventData ComponentVersionInformation { get; set; }
        public InstitutionInformationEventData InstitutionInformation { get; set; }

        public ComponentVersionManufacturerMarketingInformationEventData() { }

        public ComponentVersionManufacturerMarketingInformationEventData(
            ComponentInformationEventData componentVersionInformation,
            InstitutionInformationEventData institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
        }

        public bool IsValid()
        {
            return ComponentVersionInformation.IsValid() &&
              InstitutionInformation.IsValid();
        }
    }
}