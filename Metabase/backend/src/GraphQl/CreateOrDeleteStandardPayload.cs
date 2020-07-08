using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteStandardPayload
      : Payload
    {
        public Id StandardId { get; }

        protected CreateOrDeleteStandardPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            StandardId = timestampedId.Id;
        }
    }
}