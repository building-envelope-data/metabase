using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemovePersonAffiliationInput
    {
        public Id PersonId { get; }
        public Id InstitutionId { get; }

        public AddOrRemovePersonAffiliationInput(
            Id personId,
            Id institutionId
            )
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }
    }
}