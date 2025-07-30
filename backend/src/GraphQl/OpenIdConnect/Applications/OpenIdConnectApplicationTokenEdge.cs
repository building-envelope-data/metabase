using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationTokenEdge(
    OpenIdConnectToken node
)
{
    public OpenIdConnectToken Node { get; } = node;
}