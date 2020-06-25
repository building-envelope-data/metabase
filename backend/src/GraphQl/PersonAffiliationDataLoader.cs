using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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