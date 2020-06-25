namespace Icon.GraphQl
{
    public abstract class AddOrRemovePersonAffiliationInput
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public AddOrRemovePersonAffiliationInput(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId
            )
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }
    }
}