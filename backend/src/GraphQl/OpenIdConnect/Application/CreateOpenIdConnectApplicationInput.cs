using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed record CreateOpenIdConnectApplicationInput(
    Guid AssociatedInstitutionId,
    string ClientId,
    string DisplayName,
    OpenIdConnectConsentType ConsentType,
    Uri? RedirectUri,
    Uri? PostLogoutRedirectUri,
    IReadOnlyList<OpenIdConnectScope> Scopes
);