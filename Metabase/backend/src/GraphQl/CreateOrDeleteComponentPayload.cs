using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteComponentPayload
      : Payload
    {
        public Id ComponentId { get; }

        public CreateOrDeleteComponentPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            ComponentId = timestampedId.Id;
        }
    }
}