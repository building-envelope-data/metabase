using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteInstitutionPayload
      : CreateOrDeleteInstitutionPayload
    {
        public DeleteInstitutionPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}