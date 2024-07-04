using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed class ChangeInstitutionRepresentativeRoleError
    : UserErrorBase<ChangeInstitutionRepresentativeRoleErrorCode>
{
    public ChangeInstitutionRepresentativeRoleError(
        ChangeInstitutionRepresentativeRoleErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}