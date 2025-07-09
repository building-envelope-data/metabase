using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed class SetInstitutionExtrasError(
    SetInstitutionExtrasErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<SetInstitutionExtrasErrorCode>(code, message, path)
{
}