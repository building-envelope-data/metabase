using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;
using Uri = System.Uri;

namespace Metabase.GraphQl
{
    public sealed class CreateMethodInput
    {
        public string Name { get; }
        public string Description { get; }
        public Id? StandardId { get; }
        public Uri? PublicationLocator { get; }
        public Uri? CodeLocator { get; }
        public IReadOnlyCollection<ValueObjects.MethodCategory> Categories { get; }

        public CreateMethodInput(
            string name,
            string description,
            Id? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IReadOnlyCollection<ValueObjects.MethodCategory> categories
            )
        {
            Name = name;
            Description = description;
            StandardId = standardId;
            PublicationLocator = publicationLocator;
            CodeLocator = codeLocator;
            Categories = categories;
        }

        public static Result<ValueObjects.CreateMethodInput, Errors> Validate(
            CreateMethodInput self,
            IReadOnlyList<object> path
            )
        {
            var nameResult = ValueObjects.Name.From(
                self.Name,
                path.Append("name").ToList().AsReadOnly()
                );
            var descriptionResult = ValueObjects.Description.From(
                self.Description,
                path.Append("description").ToList().AsReadOnly()
                );
            var publicationLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(
                self.PublicationLocator,
                path.Append("publicationLocator").ToList().AsReadOnly()
                );
            var codeLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(
                self.CodeLocator,
                path.Append("codeLocator").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  nameResult,
                  descriptionResult,
                  publicationLocatorResult,
                  codeLocatorResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateMethodInput.From(
                    name: nameResult.Value,
                    description: descriptionResult.Value,
                    standardId: self.StandardId,
                    publicationLocator: publicationLocatorResult?.Value,
                    codeLocator: codeLocatorResult?.Value,
                    categories: self.Categories
                    )
                  );
        }
    }
}