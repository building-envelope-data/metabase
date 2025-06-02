using System;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed record DeleteOpenIdConnectApplicationInput(
    Guid ApplicationId
);