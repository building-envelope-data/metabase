using CSharpFunctionalExtensions;
using Icon.Infrastructure.Models;

namespace Icon.Models
{
    public sealed class MethodDeveloper
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        public ValueObjects.Id ParentId { get => MethodId; }
        public ValueObjects.Id AssociateId { get => StakeholderId; }

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