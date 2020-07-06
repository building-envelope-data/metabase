using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeletePhotovoltaicDataInput
      : DeleteNodeInput
    {
        public DeletePhotovoltaicDataInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}