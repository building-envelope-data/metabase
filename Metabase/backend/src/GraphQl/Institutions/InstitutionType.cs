using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionType
      : EntityType<Data.Institution, InstitutionByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Institution> descriptor
            )
        {
            base.Configure(descriptor);
        }
    }
}
