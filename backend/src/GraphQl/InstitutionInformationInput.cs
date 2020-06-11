using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Uri = System.Uri;

namespace Icon.GraphQl
{
    public sealed class InstitutionInformationInput
    {
        public string Name { get; }
        public string? Abbreviation { get; }
        public string? Description { get; }
        public Uri? WebsiteLocator { get; }

        public InstitutionInformationInput(
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

        public static Result<ValueObjects.InstitutionInformation, Errors> Validate(
            InstitutionInformationInput self,
            IReadOnlyList<object> path
            )
        {
            var nameResult = ValueObjects.Name.From(
                self.Name,
                path.Append("name").ToList().AsReadOnly()
                );
            var abbreviationResult = ValueObjects.Abbreviation.MaybeFrom(
                self.Abbreviation,
                path.Append("abbreviation").ToList().AsReadOnly()
                );
            var descriptionResult = ValueObjects.Description.MaybeFrom(
                self.Description,
                path.Append("description").ToList().AsReadOnly()
                );
            var websiteLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(
                self.WebsiteLocator,
                path.Append("websiteLocator").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  nameResult,
                  abbreviationResult,
                  descriptionResult,
                  websiteLocatorResult
                  )
              .Bind(_ =>
                  ValueObjects.InstitutionInformation.From(
                    name: nameResult.Value,
                    abbreviation: abbreviationResult?.Value,
                    description: descriptionResult?.Value,
                    websiteLocator: websiteLocatorResult?.Value
                    )
                  );
        }
    }
}