using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenAuthorizationEdge(
    OpenIdConnectAuthorization node
    )
{
    public OpenIdConnectAuthorization Node { get; } = node;
}