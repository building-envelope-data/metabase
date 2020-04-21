using System.Collections.Generic;
using System.Linq;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Uri = System.Uri;
using Guid = System.Guid;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public class CreateDatabaseInput
    {
        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }
        public Guid InstitutionId { get; }

        private CreateDatabaseInput(
            string name,
            string description,
            Uri locator,
            Guid institutionId
            )
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.CreateDatabaseInput, Errors> Validate(
            CreateDatabaseInput self,
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
            var locatorResult = ValueObjects.AbsoluteUri.From(
                self.Locator,
                path.Append("locator").ToList().AsReadOnly()
                );
            var institutionIdResult = ValueObjects.Id.From(
                self.InstitutionId,
                path.Append("institutionId").ToList().AsReadOnly()
                );

            return
              Errors.Combine(
                  nameResult,
                  descriptionResult,
                  locatorResult,
                  institutionIdResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateDatabaseInput.From(
                    name: nameResult.Value,
                    description: descriptionResult.Value,
                    locator: locatorResult.Value,
                    institutionId: institutionIdResult.Value
                    )
                  );
        }
    }
}