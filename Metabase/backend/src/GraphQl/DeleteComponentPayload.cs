using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteComponentPayload
      : CreateOrDeleteComponentPayload
    {
        public DeleteComponentPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}