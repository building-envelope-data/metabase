namespace Icon.Models
{
    public abstract class DataX<TDataJson>
      : Model
    {
        public ValueObjects.Id ComponentId { get; }
        public TDataJson Data { get; }

        protected DataX(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            TDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            Data = data;
        }
    }
}