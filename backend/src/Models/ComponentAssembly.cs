using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentAssembly
      : Model
    {
        public ValueObjects.Id SuperComponentId { get; }
        public ValueObjects.Id SubComponentId { get; }

        private ComponentAssembly(
            ValueObjects.Id id,
            ValueObjects.Id superComponentId,
            ValueObjects.Id subComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            SuperComponentId = superComponentId;
            SubComponentId = subComponentId;
        }

        public static Result<ComponentAssembly, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id superComponentId,
            ValueObjects.Id subComponentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentAssembly, Errors>(
                  new ComponentAssembly(
            id: id,
            superComponentId: superComponentId,
            subComponentId: subComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}