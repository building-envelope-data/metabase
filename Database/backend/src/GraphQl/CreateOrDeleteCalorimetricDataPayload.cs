using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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