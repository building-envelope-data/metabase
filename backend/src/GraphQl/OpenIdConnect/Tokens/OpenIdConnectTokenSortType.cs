using HotChocolate.Data.Sorting;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenSortType
    : AuditableEntitySortType<OpenIdConnectToken>
{
    protected override void Configure(
        ISortInputTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectTokenSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        descriptor.Field(_ => _.CreationDate).Ignore();
        descriptor.Field(_ => _.ExpirationDate).Name(OpenIdConnectTokenType.ExpiredAtName);
        descriptor.Field(_ => _.RedemptionDate).Name(OpenIdConnectTokenType.RedeemedAtName);
        descriptor.Field(_ => _.Status);
        descriptor.Field(_ => _.Subject);
        descriptor.Field(_ => _.Type);
    }
}
