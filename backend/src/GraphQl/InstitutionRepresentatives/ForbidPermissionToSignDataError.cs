using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed class ForbidPermissionToSignDataError(
    ForbidPermissionToSignDataErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<ForbidPermissionToSignDataErrorCode>(code, message, path)
{
}