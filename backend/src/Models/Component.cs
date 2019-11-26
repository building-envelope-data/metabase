using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class Component
      : Model
    {
        public ValueObjects.ComponentInformation Information { get; }

        private Component(
            ValueObjects.Id id,
            ValueObjects.ComponentInformation information,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }

        public static Result<Component, Errors> From(
            ValueObjects.Id id,
            ValueObjects.ComponentInformation information,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Component, Errors>(
                  new Component(
            id: id,
            information: information,
            timestamp: timestamp
            )
                  );
        }
    }
}