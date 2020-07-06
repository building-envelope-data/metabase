using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteUserPayload
      : Payload
    {
        public Id UserId { get; }

        public CreateOrDeleteUserPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            UserId = timestampedId.Id;
        }
    }
}