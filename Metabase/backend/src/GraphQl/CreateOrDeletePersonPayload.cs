using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeletePersonPayload
      : Payload
    {
        public Id PersonId { get; }

        protected CreateOrDeletePersonPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PersonId = timestampedId.Id;
        }
    }
}