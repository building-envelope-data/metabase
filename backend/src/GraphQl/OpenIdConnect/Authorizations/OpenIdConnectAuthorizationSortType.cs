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
        descriptor.Field(_ => _.CreationDate).Ignore();
        descriptor.Field(_ => _.Status);
        descriptor.Field(_ => _.Subject);
        descriptor.Field(_ => _.Type);
    }
}
