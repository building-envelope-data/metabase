using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class UpdateOpenIdConnectApplicationError(
    UpdateOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<UpdateOpenIdConnectApplicationErrorCode>(code, message, path)
{
}