using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteMethodPayload
      : CreateOrDeleteMethodPayload
    {
        public DeleteMethodPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}