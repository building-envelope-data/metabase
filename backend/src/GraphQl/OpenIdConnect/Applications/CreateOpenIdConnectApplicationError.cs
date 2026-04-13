using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class CreateOpenIdConnectApplicationError(
    CreateOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<CreateOpenIdConnectApplicationErrorCode>(code, message, path)
{
}