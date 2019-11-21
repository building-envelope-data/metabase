using Icon;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerMarketingInformationAggregateData
      : Validatable
    {
        public static ComponentVersionManufacturerMarketingInformationAggregateData From(
            Events.ComponentVersionManufacturerMarketingInformationEventData information
            )
        {
            return new ComponentVersionManufacturerMarketingInformationAggregateData(
                componentVersionInformation: information.ComponentVersionInformation is null ? null : ComponentInformationAggregateData.From(information.ComponentVersionInformation.NotNull()),
                institutionInformation: information.InstitutionInformation is null ? null : InstitutionInformationAggregateData.From(information.InstitutionInformation.NotNull())
            );
        }

        public ComponentInformationAggregateData? ComponentVersionInformation { get; set; }
        public InstitutionInformationAggregateData? InstitutionInformation { get; set; }

        #nullable disable
        public ComponentVersionManufacturerMarketingInformationAggregateData() { }
        #nullable enable

        public ComponentVersionManufacturerMarketingInformationAggregateData(
                ComponentInformationAggregateData? componentVersionInformation,
                InstitutionInformationAggregateData? institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
        }

        public override bool IsValid()
        {
            return
              (ComponentVersionInformation?.IsValid() ?? true) &&
              (InstitutionInformation?.IsValid() ?? true);
        }

        public Models.ComponentVersionManufacturerMarketingInformation ToModel()
        {
            return new Models.ComponentVersionManufacturerMarketingInformation(
                componentVersionInformation: ComponentVersionInformation?.ToModel(),
                institutionInformation: InstitutionInformation?.ToModel()
                );
        }
    }
}