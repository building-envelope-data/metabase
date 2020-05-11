using Exception = System.Exception;
using Models = Icon.Models;
using DateTime = System.DateTime;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class StakeholderBase
      : HotChocolate.Types.UnionType<Stakeholder>
    {
        public static Stakeholder FromModel(
            Models.Stakeholder model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            // I'd like this to be with `GraphQl.Stakeholder` but interfaces
            // cannot have class methods in C#.
            if (model is Models.Institution institution)
                return Institution.FromModel(institution, requestTimestamp);
            if (model is Models.Person person)
                return Person.FromModel(person, requestTimestamp);
            throw new Exception($"The model {model} fell through");
        }
    }
}