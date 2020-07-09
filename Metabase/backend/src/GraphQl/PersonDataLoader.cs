using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class PersonDataLoader
      : ModelDataLoader<Person, Models.Person>
    {
        public PersonDataLoader(IQueryBus queryBus)
          : base(Person.FromModel, queryBus)
        {
        }
    }
}