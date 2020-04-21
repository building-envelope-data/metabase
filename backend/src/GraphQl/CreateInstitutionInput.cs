using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Uri = System.Uri;

namespace Icon.GraphQl
{
    public sealed class CreateInstitutionInput
    {
        public string Name { get; }
        public string? Abbreviation { get; }
        public string? Description { get; }
        public Uri? WebsiteLocator { get; }
        public string? PublicKey { get; }
        public ValueObjects.InstitutionState State { get; }

        private CreateInstitutionInput(
            string name,
            string? abbreviation,
            string? description,
            Uri? websiteLocator,
            string? publicKey,
            ValueObjects.InstitutionState state
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
            State = state;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.CreateInstitutionInput, Errors> Validate(
            CreateInstitutionInput self,
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
            var publicKeyResult = ValueObjects.PublicKey.MaybeFrom(
                self.PublicKey,
                path.Append("publicKey").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  nameResult,
                  abbreviationResult,
                  descriptionResult,
                  websiteLocatorResult,
                  publicKeyResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateInstitutionInput.From(
                    name: nameResult.Value,
                    abbreviation: abbreviationResult?.Value,
                    description: descriptionResult?.Value,
                    websiteLocator: websiteLocatorResult?.Value,
                    publicKey: publicKeyResult?.Value,
                    state: self.State
                    )
                  );
        }
    }
}