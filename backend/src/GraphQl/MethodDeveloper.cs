namespace Icon.GraphQl
{
    public sealed class MethodDeveloper
      : NodeBase
    {
        public static MethodDeveloper FromModel(
            Models.MethodDeveloper model,
            ValueObjects.Timestamp requestTimestamp
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

        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        public MethodDeveloper(
            ValueObjects.Id id,
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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