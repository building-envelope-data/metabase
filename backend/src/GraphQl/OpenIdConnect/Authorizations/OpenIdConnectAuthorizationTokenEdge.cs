using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationTokenEdge(
    OpenIdConnectToken node
)
{
    public OpenIdConnectToken Node { get; } = node;
}