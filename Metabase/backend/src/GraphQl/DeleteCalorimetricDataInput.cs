using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class DeleteCalorimetricDataInput
      : DeleteNodeInput
    {
        public DeleteCalorimetricDataInput(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}