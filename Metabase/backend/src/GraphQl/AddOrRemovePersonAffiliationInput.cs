using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemovePersonAffiliationInput
    {
        public Id PersonId { get; }
        public Id InstitutionId { get; }

        protected AddOrRemovePersonAffiliationInput(
            Id personId,
            Id institutionId
            )
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }
    }
}