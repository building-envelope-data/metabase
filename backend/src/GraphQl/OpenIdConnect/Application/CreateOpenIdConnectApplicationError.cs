using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class CreateOpenIdConnectApplicationError(
    CreateOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<CreateOpenIdConnectApplicationErrorCode>(code, message, path)
{
}