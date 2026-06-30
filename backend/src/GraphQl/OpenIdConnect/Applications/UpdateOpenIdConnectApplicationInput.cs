using System;
using System.Collections.Generic;
using Metabase.Enumerations;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed record UpdateOpenIdConnectApplicationInput(
    Guid ApplicationId,
    string ClientId,
    string DisplayName,
    OpenIdConnectConsentType ConsentType,
    Uri? RedirectUri,
    Uri? PostLogoutRedirectUri,
    IReadOnlyList<OpenIdConnectEndpoint> Endpoints,
    IReadOnlyList<OpenIdConnectGrantType> GrantTypes,
    IReadOnlyList<OpenIdConnectResponseType> ResponseTypes,
    IReadOnlyList<OpenIdConnectScope> Scopes
);