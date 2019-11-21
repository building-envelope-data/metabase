using Guid = System.Guid;
using DateTime = System.DateTime;

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
        {
            EnsureValid();
        }
    }
}