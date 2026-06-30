using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public class OpenIdConnectApplicationFilterType
    : AuditableEntityFilterType<OpenIdConnectApplication>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.ApplicationType);
        descriptor.Field(_ => _.ClientId);
        descriptor.Field(_ => _.ConsentType);
        descriptor.Field(_ => _.DisplayName);
        // descriptor.Field(_ => _.PostLogoutRedirectUris);
        // descriptor.Field(_ => _.RedirectUris);
        // descriptor.Field(_ => _.Requirements);
        descriptor.Field(_ => _.Owner);
    }
}
