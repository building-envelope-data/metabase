using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteHygrothermalDataPayload
      : Payload
    {
        public Id HygrothermalDataId { get; }

        public CreateOrDeleteHygrothermalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            HygrothermalDataId = timestampedId.Id;
        }
    }
}