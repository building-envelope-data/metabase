using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public abstract class Stakeholder
      : Model
    {
        public Stakeholder(
            Guid id,
            DateTime timestamp
            )
          : base(id, timestamp)
        { }
    }
}