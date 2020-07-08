using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class DeleteNodeInput
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }

        protected DeleteNodeInput(
            Id id,
            Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}