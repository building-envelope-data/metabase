using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class DataX
      : NodeBase
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