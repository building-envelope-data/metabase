using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class ResetOpenIdConnectApplicationClientSecretError(
    ResetOpenIdConnectApplicationClientSecretErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<ResetOpenIdConnectApplicationClientSecretErrorCode>(code, message, path)
{
}