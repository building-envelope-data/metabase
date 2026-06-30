using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed record OpenIdConnectApplicationIssuedTokenEdge(
    OpenIdConnectToken Node
);