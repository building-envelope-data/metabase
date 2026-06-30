using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed record OpenIdConnectAuthorizationIssuedTokenEdge(
    OpenIdConnectToken Node
);