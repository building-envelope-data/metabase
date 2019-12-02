using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersionAssembly
      : Model
    {
        public ValueObjects.Id SuperComponentVersionId { get; }
        public ValueObjects.Id SubComponentVersionId { get; }

        private ComponentVersionAssembly(
            ValueObjects.Id id,
            ValueObjects.Id superComponentVersionId,
            ValueObjects.Id subComponentVersionId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            SuperComponentVersionId = superComponentVersionId;
            SubComponentVersionId = subComponentVersionId;
        }

        public static Result<ComponentVersionAssembly, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id superComponentVersionId,
            ValueObjects.Id subComponentVersionId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentVersionAssembly, Errors>(
                  new ComponentVersionAssembly(
                    id: id,
                    superComponentVersionId: superComponentVersionId,
                    subComponentVersionId: subComponentVersionId,
                    timestamp: timestamp
                    )
                  );
        }
    }
}