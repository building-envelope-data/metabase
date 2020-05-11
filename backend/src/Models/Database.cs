using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class Database
      : Model
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.Description Description { get; }
        public ValueObjects.AbsoluteUri Locator { get; }
        public ValueObjects.Id InstitutionId { get; }

        private Database(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }

        public static Result<Database, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Database, Errors>(
                  new Database(
            id: id,
            name: name,
            description: description,
            locator: locator,
            institutionId: institutionId,
            timestamp: timestamp
            )
                  );
        }
    }
}