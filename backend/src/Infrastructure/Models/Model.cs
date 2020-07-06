namespace Icon.Infrastructure.Models
{
    public abstract class Model
      : IModel
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        public Model(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}