using System;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed record ResetOpenIdConnectApplicationClientSecretInput(
    Guid ApplicationId
);