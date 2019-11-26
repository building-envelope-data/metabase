using Icon;
using CSharpFunctionalExtensions;
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

        public override Result<bool, Errors> Validate()
        {
          return
            Result.Combine(
                ComponentVersionInformation?.Validate() ?? Result.Ok<bool, Errors>(true),
                InstitutionInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                );
        }

        public ValueObjects.ComponentVersionManufacturerMarketingInformation ToValueObject()
        {
          var componentVersionInformationResult = ComponentVersionInformation?.ToValueObject();
          var institutionInformationResult = InstitutionInformation?.ToValueObject();

          var errors = Errors.From(
              componentVersionInformationResult ?? Result.Ok<bool, Errors>(true),
              institutionInformationResult ?? Result.Ok<bool, Errors>(true)
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.ComponentVersionManufacturerMarketingInformation, Errors>(errors);

            return ValueObjects.ComponentVersionManufacturerMarketingInformation.From(
                componentVersionInformation: componentVersionInformation?.Value,
                institutionInformation: institutionInformationResult?.Value
                );
        }
    }
}