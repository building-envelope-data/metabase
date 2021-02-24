using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Persons
{
    public sealed class PersonType
      : EntityType<Data.Person, PersonByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Person> descriptor
            )
        {
            base.Configure(descriptor);
        }
    }
}