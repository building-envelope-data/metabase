using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerMarketingInformationAggregateData
    {
        public ComponentInformationAggregateData ComponentVersionInformation { get; set; }
        public InstitutionInformationAggregateData InstitutionInformation { get; set; }

        public ComponentVersionManufacturerMarketingInformationAggregateData() { }

        public ComponentVersionManufacturerMarketingInformationAggregateData(Events.ComponentVersionManufacturerMarketingInformationEventData information)
        {
            ComponentVersionInformation = new ComponentInformationAggregateData(information.ComponentVersionInformation);
            InstitutionInformation = new InstitutionInformationAggregateData(information.InstitutionInformation);
        }

        public Models.ComponentVersionManufacturerMarketingInformation ToModel()
        {
            return new Models.ComponentVersionManufacturerMarketingInformation(
                componentVersionInformation: ComponentVersionInformation.ToModel(),
                institutionInformation: InstitutionInformation.ToModel()
                );
        }
    }
}