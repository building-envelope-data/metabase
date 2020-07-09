using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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