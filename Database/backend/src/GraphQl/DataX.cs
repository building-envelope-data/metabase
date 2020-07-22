using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Database.GraphQl
{
    public abstract class DataX
      : Node
    {
        public Id ComponentId { get; }
        public object Data { get; }

        protected DataX(
            Id id,
            Id componentId,
            object data,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            ComponentId = componentId;
            Data = data;
        }
    }
}