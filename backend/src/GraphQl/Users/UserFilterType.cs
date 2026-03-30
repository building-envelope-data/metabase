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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        // TODO The commented fields below should be filterable by OpenId Connect Clients and application users with the proper scopes and rights. If they are filterable in general, it is a way to figure out that information even if it is not returned by GraphQL.
        descriptor.Field(x => x.Name);
        // descriptor.Field(x => x.Email);
        // descriptor.Field(x => x.PostalAddress);
        // descriptor.Field(x => x.WebsiteLocator);
        descriptor.Field(x => x.DevelopedMethods);
        descriptor.Field(x => x.DevelopedMethodEdges);
        descriptor.Field(x => x.RepresentedInstitutions);
        descriptor.Field(x => x.RepresentedInstitutionEdges);
    }
}