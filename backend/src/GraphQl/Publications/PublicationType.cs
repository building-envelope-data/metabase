using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.Publications;

public sealed class PublicationType
    : ObjectType<Publication>
{
    protected override void Configure(
        IObjectTypeDescriptor<Publication> descriptor
    )
    {
    }
}