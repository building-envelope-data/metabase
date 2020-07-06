using Infrastructure.Models;
using Infrastructure.ValueObjects;

namespace Metabase.Models
{
    public abstract class Stakeholder
      : Model
    {
        protected Stakeholder(
            Id id,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}