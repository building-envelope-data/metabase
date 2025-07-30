using System;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed record DeleteOpenIdConnectApplicationInput(
    Guid ApplicationId
);