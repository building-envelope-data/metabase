using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Metabase.GraphQl
{
    public sealed class StakeholderBase
      : HotChocolate.Types.UnionType<Stakeholder>
    {
        public static Stakeholder FromModel(
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
    }
}