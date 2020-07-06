using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteMethodPayload
      : Payload
    {
        public Id MethodId { get; }

        public CreateOrDeleteMethodPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            MethodId = timestampedId.Id;
        }
    }
}