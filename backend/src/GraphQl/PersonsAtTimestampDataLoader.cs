using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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