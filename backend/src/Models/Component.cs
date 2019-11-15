using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class Component
      : Model
    {
        public ComponentInformation Information { get; }

        public Component(
            Guid id,
            ComponentInformation information,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
        }
    }
}