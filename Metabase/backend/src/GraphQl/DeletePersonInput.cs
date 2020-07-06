using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeletePersonInput
      : DeleteNodeInput
    {
        public DeletePersonInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}