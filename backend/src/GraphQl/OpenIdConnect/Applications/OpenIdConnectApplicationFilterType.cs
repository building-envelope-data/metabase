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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.ApplicationType);
        descriptor.Field(x => x.ClientId);
        descriptor.Field(x => x.ConsentType);
        descriptor.Field(x => x.DisplayName);
        // descriptor.Field(x => x.PostLogoutRedirectUris);
        // descriptor.Field(x => x.RedirectUris);
        // descriptor.Field(x => x.Requirements);
        descriptor.Field(x => x.Owner);
    }
}