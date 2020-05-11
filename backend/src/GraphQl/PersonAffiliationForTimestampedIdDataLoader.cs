using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class PersonAffiliationForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<PersonAffiliation, Models.PersonAffiliation>
    {
        public PersonAffiliationForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(PersonAffiliation.FromModel, queryBus)
        {
        }
    }
}