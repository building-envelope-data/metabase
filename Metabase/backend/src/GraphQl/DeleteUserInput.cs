using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteUserInput
      : DeleteNodeInput
    {
        public DeleteUserInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}