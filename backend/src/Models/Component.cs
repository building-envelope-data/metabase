using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class Component
      : Model
    {
        public Guid InformationId { get; }

        public Component(
            Guid id,
            Guid informationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            InformationId = informationId;
        }
    }
}