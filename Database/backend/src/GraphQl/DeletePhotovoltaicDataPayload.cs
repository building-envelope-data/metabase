using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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