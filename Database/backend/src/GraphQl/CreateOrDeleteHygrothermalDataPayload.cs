using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
{
    public abstract class CreateOrDeleteHygrothermalDataPayload
      : Payload
    {
        public Id HygrothermalDataId { get; }

        protected CreateOrDeleteHygrothermalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            HygrothermalDataId = timestampedId.Id;
        }
    }
}