using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class PersonAffiliationDataLoader
      : ModelDataLoader<PersonAffiliation, Models.PersonAffiliation>
    {
        public PersonAffiliationDataLoader(IQueryBus queryBus)
          : base(PersonAffiliation.FromModel, queryBus)
        {
        }
    }
}