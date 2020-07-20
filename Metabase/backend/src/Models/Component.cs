using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class Component
      : Model
    {
        public ValueObjects.ComponentInformation Information { get; }

        private Component(
            Id id,
            ValueObjects.ComponentInformation information,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }

        public static Result<Component, Errors> From(
            Id id,
            ValueObjects.ComponentInformation information,
            Timestamp timestamp
            )
        {
            return
              Result.Success<Component, Errors>(
                  new Component(
            id: id,
            information: information,
            timestamp: timestamp
            )
                  );
        }
    }
}