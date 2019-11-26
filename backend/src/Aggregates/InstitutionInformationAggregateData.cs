using Uri = System.Uri;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class InstitutionInformationAggregateData
      : Validatable
    {
        public static InstitutionInformationAggregateData From(
            Events.InstitutionInformationEventData information
            )
        {
            return new InstitutionInformationAggregateData(
                name: information.Name,
                abbreviation: information.Abbreviation,
                description: information.Description,
                websiteLocator: information.WebsiteLocator
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public Uri? WebsiteLocator { get; set; }

#nullable disable
        public InstitutionInformationAggregateData() { }
#nullable enable

        public InstitutionInformationAggregateData(
            string name,
            string? abbreviation,
            string? description,
            Uri? websiteLocator
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  ValidateNonNull(Name, nameof(Name))
                  );
        }

        public ValueObjects.InstitutionInformation ToValueObject()
        {
          var nameResult = ValueObjects.Name.From(Name);
          var abbreviationResult = ValueObjects.Abbreviation.From(Abbreviation);
          var descriptionResult = ValueObjects.Description.From(Description);
          var websiteLocatorResult = ValueObjects.AbsoluteUri.From(WebsiteLocator);

          var errors = Errors.From(
              nameResult,
              abbreviationResult,
              descriptionResult,
              websiteLocatorResult
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.InstitutionInformation, Errors>(errors);

            return ValueObjects.InstitutionInformation.From(
              name: nameResult.Value,
              abbreviation: abbreviationResult.Value,
              description: descriptionResult.Value,
                websiteLocator: websiteLocatorResult.Value
                );
        }
    }
}