using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed class SwitchInstitutionOperatingStateError
    : UserErrorBase<SwitchInstitutionOperatingStateErrorCode>
{
    public SwitchInstitutionOperatingStateError(
        SwitchInstitutionOperatingStateErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}