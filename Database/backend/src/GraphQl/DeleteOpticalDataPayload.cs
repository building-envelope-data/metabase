using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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