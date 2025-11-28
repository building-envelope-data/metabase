using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.References;

namespace Metabase.GraphQl.DescriptionOrReferences;

public sealed class DescriptionOrReferenceType
    : ObjectType<DescriptionOrReference>
{
    protected override void Configure(
        IObjectTypeDescriptor<DescriptionOrReference> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.Exists)
            .Ignore();
        descriptor
            .Field(t => t.Reference)
            .Type<ReferenceType>()
            .Resolve(context => context
                .Parent<DescriptionOrReference>()
                .Reference?
                .TheReference
            );
    }
}