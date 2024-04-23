using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed class CreateInstitutionError
    : UserErrorBase<CreateInstitutionErrorCode>
{
    public CreateInstitutionError(
        CreateInstitutionErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}