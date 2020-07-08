using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteInstitutionPayload
      : Payload
    {
        public Id InstitutionId { get; }

        protected CreateOrDeleteInstitutionPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            InstitutionId = timestampedId.Id;
        }
    }
}