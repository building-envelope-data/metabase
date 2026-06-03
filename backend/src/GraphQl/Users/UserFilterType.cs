using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Users;

public sealed class UserFilterType
    : AuditableEntityFilterType<User>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<User> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        // TODO The commented fields below should be filterable by OpenId Connect Clients and application users with the proper scopes and rights. If they are filterable in general, it is a way to figure out that information even if it is not returned by GraphQL.
        descriptor.Field(_ => _.Name);
        // descriptor.Field(_ => _.Email);
        // descriptor.Field(_ => _.PostalAddress);
        // descriptor.Field(_ => _.WebsiteLocator);
        descriptor.Field(_ => _.DevelopedMethods);
        descriptor.Field(_ => _.DevelopedMethodEdges);
        descriptor.Field(_ => _.RepresentedInstitutions);
        descriptor.Field(_ => _.RepresentedInstitutionEdges);
    }
}
