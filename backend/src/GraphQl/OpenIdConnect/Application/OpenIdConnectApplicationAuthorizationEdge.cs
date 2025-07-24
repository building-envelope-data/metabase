using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationAuthorizationEdge(
    OpenIdConnectAuthorization node
)
{
    public OpenIdConnectAuthorization Node { get; } = node;
}