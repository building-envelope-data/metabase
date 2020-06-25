namespace Icon.GraphQl
{
    public abstract class AddOrRemoveInstitutionRepresentativeInput
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }

        public AddOrRemoveInstitutionRepresentativeInput(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId
            )
        {
            InstitutionId = institutionId;
            UserId = userId;
        }
    }
}