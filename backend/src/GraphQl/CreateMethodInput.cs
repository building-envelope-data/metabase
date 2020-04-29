using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Uri = System.Uri;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class CreateMethodInput
    {
        public string Name { get; }
        public string Description { get; }
        public ValueObjects.Id? StandardId { get; }
        public Uri? PublicationLocator { get; }
        public Uri? CodeLocator { get; }
        public IReadOnlyCollection<ValueObjects.MethodCategory> Categories { get; }

        public CreateMethodInput(
            string name,
            string description,
            ValueObjects.Id? standardId,
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

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
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