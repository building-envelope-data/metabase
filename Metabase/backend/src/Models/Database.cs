using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class Database
      : Model
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.Description Description { get; }
        public ValueObjects.AbsoluteUri Locator { get; }

        private Database(
            Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Description = description;
            Locator = locator;
        }

        public static Result<Database, Errors> From(
            Id id,
            ValueObjects.Name name,
            ValueObjects.Description description,
            ValueObjects.AbsoluteUri locator,
            Timestamp timestamp
            )
        {
            return
              Result.Success<Database, Errors>(
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