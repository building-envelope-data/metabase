using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerMarketingInformationAggregateData
    {
        public static ComponentVersionManufacturerMarketingInformationAggregateData From(
            Events.ComponentVersionManufacturerMarketingInformationEventData information
            )
        {
            return new ComponentVersionManufacturerMarketingInformationAggregateData(
                componentVersionInformation: ComponentInformationAggregateData.From(information.ComponentVersionInformation),
                institutionInformation: InstitutionInformationAggregateData.From(information.InstitutionInformation)
            );
        }

        public ComponentInformationAggregateData ComponentVersionInformation { get; set; }
        public InstitutionInformationAggregateData InstitutionInformation { get; set; }

        public ComponentVersionManufacturerMarketingInformationAggregateData() { }

        public ComponentVersionManufacturerMarketingInformationAggregateData(
                ComponentInformationAggregateData componentVersionInformation,
                InstitutionInformationAggregateData institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
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