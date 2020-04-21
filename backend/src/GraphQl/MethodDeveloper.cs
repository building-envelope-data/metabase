using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

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

        public Guid MethodId { get; }
        public Guid StakeholderId { get; }

        public MethodDeveloper(
            Guid id,
            Guid methodId,
            Guid stakeholderId,
            DateTime timestamp,
            DateTime requestTimestamp
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