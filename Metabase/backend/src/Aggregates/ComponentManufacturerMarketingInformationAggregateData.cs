using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using Validatable = Infrastructure.Validatable;

namespace Metabase.Aggregates
{
    public sealed class ComponentManufacturerMarketingInformationAggregateData
      : Validatable
    {
        public static ComponentManufacturerMarketingInformationAggregateData From(
            Events.ComponentManufacturerMarketingInformationEventData information
            )
        {
            return new ComponentManufacturerMarketingInformationAggregateData(
                componentInformation: information.ComponentInformation is null ? null : ComponentInformationAggregateData.From(information.ComponentInformation),
                institutionInformation: information.InstitutionInformation is null ? null : InstitutionInformationAggregateData.From(information.InstitutionInformation)
            );
        }

        public ComponentInformationAggregateData? ComponentInformation { get; set; }
        public InstitutionInformationAggregateData? InstitutionInformation { get; set; }

#nullable disable
        public ComponentManufacturerMarketingInformationAggregateData() { }
#nullable enable

        public ComponentManufacturerMarketingInformationAggregateData(
                ComponentInformationAggregateData? componentInformation,
                InstitutionInformationAggregateData? institutionInformation
            )
        {
            ComponentInformation = componentInformation;
            InstitutionInformation = institutionInformation;
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  ComponentInformation?.Validate() ?? Result.Ok<bool, Errors>(true),
                  InstitutionInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }

        public Result<ValueObjects.ComponentManufacturerMarketingInformation, Errors> ToValueObject()
        {
            var componentInformationResult = ComponentInformation?.ToValueObject();
            var institutionInformationResult = InstitutionInformation?.ToValueObject();

            return
              Errors.CombineExistent(
                  componentInformationResult,
                  institutionInformationResult
                  )
              .Bind(_ =>
                  ValueObjects.ComponentManufacturerMarketingInformation.From(
                    componentInformation: componentInformationResult?.Value,
                    institutionInformation: institutionInformationResult?.Value
                    )
                  );
        }
    }
}