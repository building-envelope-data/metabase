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
        descriptor.Field(_ => _.ApplicationType);
        descriptor.Field(_ => _.ClientId);
        descriptor.Field(_ => _.ConsentType);
        descriptor.Field(_ => _.DisplayName);
        // descriptor.Field(_ => _.PostLogoutRedirectUris);
        // descriptor.Field(_ => _.RedirectUris);
    }
}
