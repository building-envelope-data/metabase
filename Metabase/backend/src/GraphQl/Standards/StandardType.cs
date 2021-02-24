using HotChocolate.Types;

namespace Metabase.GraphQl.Standards
{
    public sealed class StandardType
      : EntityType<Data.Standard, StandardByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Standard> descriptor
            )
        {
            base.Configure(descriptor);
        }
    }
}