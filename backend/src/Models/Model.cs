using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public abstract class Model
      : Validatable
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        public Model(
            Guid id,
            DateTime timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              Id != Guid.Empty &&
              Timestamp != DateTime.MinValue;
        }
    }
}