using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
{
    public sealed class DeleteHygrothermalDataPayload
      : CreateOrDeleteHygrothermalDataPayload
    {
        public DeleteHygrothermalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}