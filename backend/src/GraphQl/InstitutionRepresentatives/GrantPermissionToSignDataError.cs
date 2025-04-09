using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed class GrantPermissionToSignDataError(
    GrantPermissionToSignDataErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<GrantPermissionToSignDataErrorCode>(code, message, path)
{
}