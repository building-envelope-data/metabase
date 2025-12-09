using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationFilterType
    : FilterInputType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Id).Name("uuid");
        descriptor.Field(x => x.CreationDate);
        descriptor.Field(x => x.Status);
        descriptor.Field(x => x.Subject);
        descriptor.Field(x => x.Tokens);
        descriptor.Field(x => x.Type);
        // descriptor.Field(x => x.Application);
    }
}