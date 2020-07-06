using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteInstitutionInput
      : DeleteNodeInput
    {
        public DeleteInstitutionInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}