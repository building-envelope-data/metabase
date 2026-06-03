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
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.CreationDate).Ignore();
        descriptor.Field(_ => _.Status);
        descriptor.Field(_ => _.Subject);
        descriptor.Field(_ => _.Tokens);
        descriptor.Field(_ => _.Type);
        descriptor.Field(_ => _.Application);
    }
}
