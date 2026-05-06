using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperSortType
    : AuditableAssociationSortType<IMethodDeveloper>
{
    protected override void Configure(
        ISortInputTypeDescriptor<IMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(MethodDeveloperSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        // Disjunctively compose the sort following sort taking into
        // account the "lifting" done in `MethodDeveloperConnection`.
        // descriptor
        //     .Field(nameof(InstitutionMethodDeveloper.Institution))
        //     .Type<InstitutionSortType>();
        // descriptor
        //     .Field(nameof(UserMethodDeveloper.User))
        //     .Type<UserSortType>();
    }
}