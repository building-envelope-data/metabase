using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Users;

public sealed class UserSortType
    : AuditableEntitySortType<User>
{
    protected override void Configure(
        ISortInputTypeDescriptor<User> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        // TODO The commented fiels below should be sortable by OpenId Connect Clients and application users with the proper scopes and rights. If they are sortable in general, it is a way to figure out that information even if it is not returned by GraphQL.
        descriptor.Field(_ => _.Name);
        // descriptor.Field(_ => _.Email);
        // descriptor.Field(_ => _.PostalAddress);
        // descriptor.Field(_ => _.WebsiteLocator);
    }
}
