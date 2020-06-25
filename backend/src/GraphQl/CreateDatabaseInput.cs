using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Uri = System.Uri;

namespace Icon.GraphQl
{
    public class CreateDatabaseInput
    {
        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }
        public ValueObjects.Id InstitutionId { get; }

        private CreateDatabaseInput(
            string name,
            string description,
            Uri locator,
            ValueObjects.Id institutionId
            )
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }

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

            return
              Errors.Combine(
                  nameResult,
                  descriptionResult,
                  locatorResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateDatabaseInput.From(
                    name: nameResult.Value,
                    description: descriptionResult.Value,
                    locator: locatorResult.Value,
                    institutionId: self.InstitutionId
                    )
                  );
        }
    }
}