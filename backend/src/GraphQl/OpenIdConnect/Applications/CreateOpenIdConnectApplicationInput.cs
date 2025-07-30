using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed record CreateOpenIdConnectApplicationInput(
    Guid InstitutionId,
    string ClientId,
    string DisplayName,
    OpenIdConnectConsentType ConsentType,
    Uri? RedirectUri,
    Uri? PostLogoutRedirectUri,
    IReadOnlyList<OpenIdConnectScope> Scopes
);