using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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