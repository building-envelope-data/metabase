using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
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