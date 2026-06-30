using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Scalars;

namespace Metabase.GraphQl.Associations;

public abstract class AssociationType<TAssociation>
    : ObjectType<TAssociation>
    where TAssociation : IAssociation
{
    protected override void Configure(
        IObjectTypeDescriptor<TAssociation> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Do we want to expose this, require it as input, and use it to discover concurrent writes?
        descriptor
            .Field(t => t.Version)
            .Type<NonNullType<NonNegativeIntType>>()
            .Name(GraphQlConstants.VersionFieldName)
            .Ignore();
    }
}