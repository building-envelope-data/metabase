using HotChocolate.Types;

namespace Metabase.GraphQl.Publications
{
    public sealed class PublicationType
      : ObjectType<Data.Publication>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Publication> descriptor
            )
        {
        }
    }
}