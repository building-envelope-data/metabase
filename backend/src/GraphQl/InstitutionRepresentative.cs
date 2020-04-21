using ValueObjects = Icon.ValueObjects;
using Guid = System.Guid;
using DateTime = System.DateTime;

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

        public Guid InstitutionId { get; }
        public Guid UserId { get; }
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentative(
            Guid id,
            Guid institutionId,
            Guid userId,
            ValueObjects.InstitutionRepresentativeRole role,
            DateTime timestamp,
            DateTime requestTimestamp
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