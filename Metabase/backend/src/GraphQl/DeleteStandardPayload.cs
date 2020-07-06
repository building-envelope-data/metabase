using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteStandardPayload
      : CreateOrDeleteStandardPayload
    {
        public DeleteStandardPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}