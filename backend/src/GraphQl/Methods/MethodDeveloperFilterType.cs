using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperFilterType
    : AuditableAssociationFilterType<IMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<IMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(MethodDeveloperFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        // Disjunctively compose the filters following filters taking into
        // account the "lifting" done in `MethodDeveloperConnection`.
        // descriptor
        //     .Field(nameof(InstitutionMethodDeveloper.Institution))
        //     .Type<InstitutionFilterType>();
        // descriptor
        //     .Field(nameof(UserMethodDeveloper.User))
        //     .Type<UserFilterType>();
    }
}