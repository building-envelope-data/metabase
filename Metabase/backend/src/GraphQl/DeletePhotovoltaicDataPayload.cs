using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeletePhotovoltaicDataPayload
      : CreateOrDeletePhotovoltaicDataPayload
    {
        public DeletePhotovoltaicDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}