using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class PersonsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Person, Models.Person>
    {
        public PersonsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Person.FromModel, queryBus)
        {
        }
    }
}