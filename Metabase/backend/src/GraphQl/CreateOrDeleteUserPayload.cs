using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteUserPayload
      : Payload
    {
        public Id UserId { get; }

        protected CreateOrDeleteUserPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            UserId = timestampedId.Id;
        }
    }
}