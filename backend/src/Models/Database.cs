using CSharpFunctionalExtensions;
using Icon.Infrastructure.Models;

namespace Icon.Models
{
    public sealed class Database
      : Model
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.Description Description { get; }
        public ValueObjects.AbsoluteUri Locator { get; }

        private Database(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Description = description;
            Locator = locator;
        }

        public static Result<Database, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
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
            timestamp: timestamp
            )
                  );
        }
    }
}