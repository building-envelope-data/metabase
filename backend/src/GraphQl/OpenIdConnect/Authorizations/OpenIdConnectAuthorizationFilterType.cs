using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationFilterType
    : EntityFilterType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectAuthorizationFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.CreationDate);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Tokens);
        descriptor.Field(x => x.Type);
        // descriptor.Field(x => x.Application);
    }
}