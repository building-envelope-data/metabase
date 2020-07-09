using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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