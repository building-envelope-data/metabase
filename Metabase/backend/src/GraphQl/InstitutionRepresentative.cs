using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class InstitutionRepresentative
      : NodeBase
    {
        public static InstitutionRepresentative FromModel(
            Models.InstitutionRepresentative model,
            Timestamp requestTimestamp
            )
        {
            return new InstitutionRepresentative(
                id: model.Id,
                institutionId: model.InstitutionId,
                userId: model.UserId,
                role: model.Role,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Id InstitutionId { get; }
        public Id UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentative(
            Id id,
            Id institutionId,
            Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            InstitutionId = institutionId;
            UserId = userId;
            Role = role;
        }
    }
}