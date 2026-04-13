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
        base.Configure(descriptor);
        descriptor
            .Field(t => t.Exists)
            .Ignore();
    }
}