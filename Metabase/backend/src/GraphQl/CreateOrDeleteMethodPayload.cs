using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteMethodPayload
      : Payload
    {
        public Id MethodId { get; }

        protected CreateOrDeleteMethodPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            MethodId = timestampedId.Id;
        }
    }
}