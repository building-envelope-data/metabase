namespace Icon.GraphQl
{
    public sealed class DeleteInstitutionInput
      : DeleteNodeInput
    {
        public DeleteInstitutionInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}