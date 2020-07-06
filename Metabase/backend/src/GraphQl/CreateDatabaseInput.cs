using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;
using Uri = System.Uri;

namespace Metabase.GraphQl
{
    public class CreateDatabaseInput
    {
        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }
        public Id InstitutionId { get; }

        public CreateDatabaseInput(
            string name,
            string description,
            Uri locator,
            Id institutionId
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