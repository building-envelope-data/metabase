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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.CreationDate).Ignore();
        descriptor.Field(x => x.ExpirationDate).Name(OpenIdConnectTokenType.ExpiredAtName);
        descriptor.Field(x => x.RedemptionDate).Name(OpenIdConnectTokenType.RedeemedAtName);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Type);
        descriptor.Field(x => x.Authorization);
        descriptor.Field(x => x.Application);
    }
}