using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class InstitutionRepresentative
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public ValueObjects.Id ParentId { get => InstitutionId; }
        public ValueObjects.Id AssociateId { get => UserId; }

        private InstitutionRepresentative(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            InstitutionId = institutionId;
            UserId = userId;
            Role = role;
        }

        public static Result<InstitutionRepresentative, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<InstitutionRepresentative, Errors>(
                  new InstitutionRepresentative(
                    id: id,
                    institutionId: institutionId,
                    userId: userId,
                    role: role,
                    timestamp: timestamp
                    )
                  );
        }
    }
}