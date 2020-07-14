using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class InstitutionRepresentative
      : Model, IManyToManyAssociation
    {
        public Id InstitutionId { get; }
        public Id UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public Id ParentId { get => InstitutionId; }
        public Id AssociateId { get => UserId; }

        private InstitutionRepresentative(
            Id id,
            Id institutionId,
            Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            InstitutionId = institutionId;
            UserId = userId;
            Role = role;
        }

        public static Result<InstitutionRepresentative, Errors> From(
            Id id,
            Id institutionId,
            Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            Timestamp timestamp
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