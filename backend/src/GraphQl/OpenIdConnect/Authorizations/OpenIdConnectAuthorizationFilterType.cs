using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationFilterType
    : AuditableEntityFilterType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectAuthorizationFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.CreationDate).Ignore();
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Tokens);
        descriptor.Field(x => x.Type);
        descriptor.Field(x => x.Application);
    }
}