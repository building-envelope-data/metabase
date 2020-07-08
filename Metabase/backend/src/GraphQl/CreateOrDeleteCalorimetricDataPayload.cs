using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteCalorimetricDataPayload
      : Payload
    {
        public Id CalorimetricDataId { get; }

        protected CreateOrDeleteCalorimetricDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            CalorimetricDataId = timestampedId.Id;
        }
    }
}