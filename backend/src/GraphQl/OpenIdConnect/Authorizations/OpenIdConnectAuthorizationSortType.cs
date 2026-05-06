using HotChocolate.Data.Sorting;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationSortType
    : AuditableEntitySortType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        ISortInputTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectAuthorizationSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        descriptor.Field(x => x.CreationDate).Ignore();
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Type);
    }
}