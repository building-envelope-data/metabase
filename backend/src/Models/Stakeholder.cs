namespace Icon.Models
{
    public abstract class Stakeholder
      : Model
    {
        protected Stakeholder(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}