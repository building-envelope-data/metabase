using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class MethodDeveloper
      : NodeBase
    {
        public static MethodDeveloper FromModel(
            Models.MethodDeveloper model,
            Timestamp requestTimestamp
            )
        {
            return new MethodDeveloper(
                id: model.Id,
                methodId: model.MethodId,
                stakeholderId: model.StakeholderId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Id MethodId { get; }
        public Id StakeholderId { get; }

        public MethodDeveloper(
            Id id,
            Id methodId,
            Id stakeholderId,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }
    }
}