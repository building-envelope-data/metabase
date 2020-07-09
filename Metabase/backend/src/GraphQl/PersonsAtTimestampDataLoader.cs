using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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