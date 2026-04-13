using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class DeleteOpenIdConnectApplicationError(
    DeleteOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<DeleteOpenIdConnectApplicationErrorCode>(code, message, path)
{
}