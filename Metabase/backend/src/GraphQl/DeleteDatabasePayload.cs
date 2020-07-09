using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteDatabasePayload
      : CreateOrDeleteDatabasePayload
    {
        public DeleteDatabasePayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}