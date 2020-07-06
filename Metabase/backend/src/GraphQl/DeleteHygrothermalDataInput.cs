using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteHygrothermalDataInput
      : DeleteNodeInput
    {
        public DeleteHygrothermalDataInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}