using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteOpticalDataPayload
      : CreateOrDeleteOpticalDataPayload
    {
        public DeleteOpticalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}