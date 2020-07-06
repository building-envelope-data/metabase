using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteInstitutionPayload
      : Payload
    {
        public Id InstitutionId { get; }

        public CreateOrDeleteInstitutionPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            InstitutionId = timestampedId.Id;
        }
    }
}