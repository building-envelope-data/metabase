using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class PersonForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Person, Models.Person>
    {
        public PersonForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Person.FromModel, queryBus)
        {
        }
    }
}