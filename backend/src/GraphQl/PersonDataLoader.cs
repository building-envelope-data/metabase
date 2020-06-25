using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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