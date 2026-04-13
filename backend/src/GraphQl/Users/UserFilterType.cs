using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Users;

public sealed class UserFilterType
    : EntityFilterType<User>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<User> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO The commented fields below should be filterable by OpenId Connect Clients and application users with the proper scopes and rights. If they are filterable in general, it is a way to figure out that information even if it is not returned by GraphQL.
        // descriptor.Field(x => x.Name);
        // descriptor.Field(x => x.Email);
        // descriptor.Field(x => x.PostalAddress);
        // descriptor.Field(x => x.WebsiteLocator);
        descriptor.Field(x => x.DevelopedMethods);
        descriptor.Field(x => x.DevelopedMethodEdges);
        descriptor.Field(x => x.RepresentedInstitutions);
        descriptor.Field(x => x.RepresentedInstitutionEdges);
    }
}