using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class InstitutionRepresentative
      : NodeBase
    {
        public static InstitutionRepresentative FromModel(
            Models.InstitutionRepresentative model,
            ValueObjects.Timestamp requestTimestamp
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

        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentative(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.InstitutionRepresentativeRole role,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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