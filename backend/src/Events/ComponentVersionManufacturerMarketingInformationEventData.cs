using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerMarketingInformationEventData
    {
        public ComponentInformationEventData ComponentVersionInformation { get; set; }
        public InstitutionInformationEventData InstitutionInformation { get; set; }

        public ComponentVersionManufacturerMarketingInformationEventData() { }

        public ComponentVersionManufacturerMarketingInformationEventData(Models.ComponentVersionManufacturerMarketingInformation information) {
          ComponentVersionInformation = new ComponentInformationEventData(information.ComponentVersionInformation);
          InstitutionInformation = new InstitutionInformationEventData(information.InstitutionInformation);
        }
    }
}
