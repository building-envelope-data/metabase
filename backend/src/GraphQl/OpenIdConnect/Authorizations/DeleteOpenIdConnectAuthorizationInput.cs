using System;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed record DeleteOpenIdConnectAuthorizationInput(
    Guid AuthorizationId
);