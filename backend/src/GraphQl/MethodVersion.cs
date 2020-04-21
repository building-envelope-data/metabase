using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class MethodVersion
      : NodeBase
    {
        public static MethodVersion FromModel(
            Models.MethodVersion model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new MethodVersion(
                id: model.Id,
                methodId: model.MethodId,
                information: MethodInformation.FromModel(model.Information),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Guid MethodId { get; }
        public MethodInformation Information { get; }

        public MethodVersion(
            Guid id,
            Guid methodId,
            MethodInformation information,
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
            Information = information;
        }
    }
}