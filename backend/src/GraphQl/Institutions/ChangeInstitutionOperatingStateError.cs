using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed class ChangeInstitutionOperatingStateError
    : UserErrorBase<ChangeInstitutionOperatingStateErrorCode>
{
    public ChangeInstitutionOperatingStateError(
        ChangeInstitutionOperatingStateErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}