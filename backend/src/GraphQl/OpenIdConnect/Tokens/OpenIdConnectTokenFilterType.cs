using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenFilterType
    : EntityFilterType<OpenIdConnectToken>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(OpenIdConnectTokenFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.CreationDate);
        descriptor.Field(x => x.ExpirationDate);
        descriptor.Field(x => x.RedemptionDate);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Type);
        // descriptor.Field(x => x.Authorization);
        // descriptor.Field(x => x.Application);
    }
}