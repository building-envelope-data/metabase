using HotChocolate.Data.Sorting;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public class OpenIdConnectApplicationSortType
    : AuditableEntitySortType<OpenIdConnectApplication>
{
    protected override void Configure(
        ISortInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.ApplicationType);
        descriptor.Field(x => x.ClientId);
        descriptor.Field(x => x.ConsentType);
        descriptor.Field(x => x.DisplayName);
        // descriptor.Field(x => x.PostLogoutRedirectUris);
        // descriptor.Field(x => x.RedirectUris);
    }
}