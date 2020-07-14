using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
{
    public abstract class CreateOrDeleteOpticalDataPayload
      : Payload
    {
        public Id OpticalDataId { get; }

        protected CreateOrDeleteOpticalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            OpticalDataId = timestampedId.Id;
        }
    }
}