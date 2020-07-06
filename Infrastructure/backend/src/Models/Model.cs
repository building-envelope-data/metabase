using Infrastructure.ValueObjects;
namespace Infrastructure.Models
{
    public abstract class Model
      : IModel
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }

        public Model(
            Id id,
            Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}