using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed class UpdateInstitutionError
    : UserErrorBase<UpdateInstitutionErrorCode>
{
    public UpdateInstitutionError(
        UpdateInstitutionErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}