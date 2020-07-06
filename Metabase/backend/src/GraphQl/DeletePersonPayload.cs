using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeletePersonPayload
      : CreateOrDeletePersonPayload
    {
        public DeletePersonPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}