using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenFilterType
    : AuditableEntityFilterType<OpenIdConnectToken>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectTokenFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.CreationDate).Ignore();
        descriptor.Field(_ => _.ExpirationDate).Name(OpenIdConnectTokenType.ExpiredAtName);
        descriptor.Field(_ => _.RedemptionDate).Name(OpenIdConnectTokenType.RedeemedAtName);
        descriptor.Field(_ => _.Status);
        descriptor.Field(_ => _.Subject);
        descriptor.Field(_ => _.Type);
        descriptor.Field(_ => _.Authorization);
        descriptor.Field(_ => _.Application);
    }
}
