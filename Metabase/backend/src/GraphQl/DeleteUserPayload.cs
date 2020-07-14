using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteUserPayload
      : CreateOrDeleteUserPayload
    {
        public DeleteUserPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}