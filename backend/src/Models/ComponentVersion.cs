using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersion
      : Model
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.ComponentInformation Information { get; }

        private ComponentVersion(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.ComponentInformation information,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            Information = information;
        }

        public static Result<ComponentVersion, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.ComponentInformation information,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentVersion, Errors>(
                  new ComponentVersion(
            id: id,
            componentId: componentId,
            information: information,
            timestamp: timestamp
            )
                  );
        }
    }
}