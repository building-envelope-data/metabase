using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public class OpenIdConnectApplicationFilterType
    : FilterInputType<OpenIdConnectApplication>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Id).Name(GraphQlConstants.UuidFieldName);
        descriptor.Field(x => x.ApplicationType);
        descriptor.Field(x => x.ClientId);
        descriptor.Field(x => x.ConsentType);
        descriptor.Field(x => x.DisplayName);
        // descriptor.Field(x => x.PostLogoutRedirectUris);
        // descriptor.Field(x => x.RedirectUris);
        // descriptor.Field(x => x.Requirements);
    }
}