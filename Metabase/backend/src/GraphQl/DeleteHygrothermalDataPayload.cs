using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
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