using Infrastructure.Models;
using Infrastructure.ValueObjects;

namespace Metabase.Models
{
    public abstract class DataX<TDataJson>
      : Model
    {
        public Id ComponentId { get; }
        public TDataJson Data { get; }

        protected DataX(
            Id id,
            Id componentId,
            TDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            Data = data;
        }
    }
}