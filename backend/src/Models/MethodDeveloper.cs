using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class MethodDeveloper
      : Model
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        private MethodDeveloper(
            ValueObjects.Id id,
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        public static Result<MethodDeveloper, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<MethodDeveloper, Errors>(
                  new MethodDeveloper(
            id: id,
            methodId: methodId,
            stakeholderId: stakeholderId,
            timestamp: timestamp
            )
                  );
        }
    }
}