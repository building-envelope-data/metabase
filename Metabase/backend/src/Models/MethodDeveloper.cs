using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class MethodDeveloper
      : Model, IManyToManyAssociation
    {
        public Id MethodId { get; }
        public Id StakeholderId { get; }

        public Id ParentId { get => MethodId; }
        public Id AssociateId { get => StakeholderId; }

        private MethodDeveloper(
            Id id,
            Id methodId,
            Id stakeholderId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        public static Result<MethodDeveloper, Errors> From(
            Id id,
            Id methodId,
            Id stakeholderId,
            Timestamp timestamp
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