using HotChocolate.Types;

namespace Metabase.GraphQl.Standards
{
    public sealed class StandardType
      : ObjectType<Data.Standard>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Standard> descriptor
            )
        {
        }
    }
}