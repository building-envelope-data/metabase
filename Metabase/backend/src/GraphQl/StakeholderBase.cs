using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Metabase.GraphQl
{
    public abstract class StakeholderBase
      : Node, IStakeholder
    {
        public static IStakeholder FromModel(
            Models.Stakeholder model,
            Timestamp requestTimestamp
            )
        {
            // I'd like this to be with `GraphQl.Stakeholder` but interfaces
            // cannot have class methods in C#.
            return model switch
            {
                Models.Institution institution =>
                  Institution.FromModel(institution, requestTimestamp),
                Models.Person person =>
                  Person.FromModel(person, requestTimestamp),
                _ =>
                  throw new Exception($"The model {model} fell through")
            };
        }

        protected StakeholderBase(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        { }
    }
}