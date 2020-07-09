using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Database.GraphQl
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