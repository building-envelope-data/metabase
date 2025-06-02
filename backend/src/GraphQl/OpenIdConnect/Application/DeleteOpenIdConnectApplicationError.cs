using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class DeleteOpenIdConnectApplicationError(
    DeleteOpenIdConnectApplicationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<DeleteOpenIdConnectApplicationErrorCode>(code, message, path)
{
}