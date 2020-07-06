using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteMethodInput
      : DeleteNodeInput
    {
        public DeleteMethodInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}