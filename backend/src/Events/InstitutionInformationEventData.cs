using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;
using Models = Icon.Models;
using Uri = System.Uri;

namespace Icon.Events
{
    public sealed class InstitutionInformationEventData
      : Validatable
    {
        public static InstitutionInformationEventData From(
            ValueObjects.InstitutionInformation information
            )
        {
            return new InstitutionInformationEventData(
                name: information.Name,
                abbreviation: information.Abbreviation?.Value,
                description: information.Description?.Value,
                websiteLocator: information.WebsiteLocator?.Value
                );
        }

        public static InstitutionInformationEventData From(
            ValueObjects.CreateInstitutionInput input
            )
        {
            return new InstitutionInformationEventData(
                name: input.Name,
                abbreviation: input.Abbreviation?.Value,
                description: input.Description?.Value,
                websiteLocator: input.WebsiteLocator?.Value
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public Uri? WebsiteLocator { get; set; }

#nullable disable
        public InstitutionInformationEventData() { }
#nullable enable

        public InstitutionInformationEventData(
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
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  ValidateNonNull(Name, nameof(Name))
                  );
        }
    }
}