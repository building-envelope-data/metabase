using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationApplicationEdge(
    OpenIdConnectApplication node
    )
{
    public OpenIdConnectApplication Node { get; } = node;
}