using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public class OpenIdConnectApplicationFilterType
    : EntityFilterType<OpenIdConnectApplication>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectApplicationFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.ApplicationType);
        descriptor.Field(x => x.ClientId);
        descriptor.Field(x => x.ConsentType);
        descriptor.Field(x => x.DisplayName);
        // descriptor.Field(x => x.PostLogoutRedirectUris);
        // descriptor.Field(x => x.RedirectUris);
        // descriptor.Field(x => x.Requirements);
    }
}