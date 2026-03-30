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
        descriptor.Field(x => x.CreationDate).Ignore();
        descriptor.Field(x => x.ExpirationDate).Name(OpenIdConnectTokenType.ExpiredAtName);
        descriptor.Field(x => x.RedemptionDate).Name(OpenIdConnectTokenType.RedeemedAtName);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Type);
    }
}