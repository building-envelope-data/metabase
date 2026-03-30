using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.UserMethodDevelopers;

public abstract class UserMethodDeveloperSortType
    : AuditableAssociationSortType<UserMethodDeveloper>
{
    protected override void Configure(
        ISortInputTypeDescriptor<UserMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserMethodDeveloperSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}