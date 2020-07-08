using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
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