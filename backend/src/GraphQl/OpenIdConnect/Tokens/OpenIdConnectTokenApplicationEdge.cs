using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenApplicationEdge(
    OpenIdConnectApplication node
    )
{
    public OpenIdConnectApplication Node { get; } = node;
}