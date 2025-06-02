using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class UpdateOpenIdConnectApplicationError(
    UpdateOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<UpdateOpenIdConnectApplicationErrorCode>(code, message, path)
{
}