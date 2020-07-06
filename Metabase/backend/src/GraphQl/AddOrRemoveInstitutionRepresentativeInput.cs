using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveInstitutionRepresentativeInput
    {
        public Id InstitutionId { get; }
        public Id UserId { get; }

        public AddOrRemoveInstitutionRepresentativeInput(
            Id institutionId,
            Id userId
            )
        {
            InstitutionId = institutionId;
            UserId = userId;
        }
    }
}