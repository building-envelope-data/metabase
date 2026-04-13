using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationAuthorizationEdge(
    OpenIdConnectAuthorization node
)
{
    public OpenIdConnectAuthorization Node { get; } = node;
}