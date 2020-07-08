using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeletePhotovoltaicDataPayload
      : Payload
    {
        public Id PhotovoltaicDataId { get; }

        protected CreateOrDeletePhotovoltaicDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PhotovoltaicDataId = timestampedId.Id;
        }
    }
}