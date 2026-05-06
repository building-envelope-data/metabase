using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.UserMethodDevelopers;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodSortType
    : UserMethodDeveloperSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<UserMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserDevelopedMethodSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}