using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationTokenEdge(
    OpenIdConnectToken node
)
{
    public OpenIdConnectToken Node { get; } = node;
}