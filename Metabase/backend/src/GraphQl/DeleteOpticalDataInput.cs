using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteOpticalDataInput
      : DeleteNodeInput
    {
        public DeleteOpticalDataInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}