using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenFilterType
    : FilterInputType<OpenIdConnectToken>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Id).Name(GraphQlConstants.UuidFieldName);
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