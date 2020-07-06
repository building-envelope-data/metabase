using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteCalorimetricDataPayload
      : CreateOrDeleteCalorimetricDataPayload
    {
        public DeleteCalorimetricDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}