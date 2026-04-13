using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class DeleteOpenIdConnectAuthorizationError(
    DeleteOpenIdConnectAuthorizationErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
: UserErrorBase<DeleteOpenIdConnectAuthorizationErrorCode>(code, message, path)
{
}