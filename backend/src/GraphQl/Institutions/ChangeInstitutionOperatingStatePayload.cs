using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class ChangeInstitutionOperatingStatePayload
{
    public ChangeInstitutionOperatingStatePayload(
        Institution institution
    )
    {
        Institution = institution;
    }

    public ChangeInstitutionOperatingStatePayload(
        IReadOnlyCollection<ChangeInstitutionOperatingStateError> errors
    )
    {
        Errors = errors;
    }

    public ChangeInstitutionOperatingStatePayload(
        ChangeInstitutionOperatingStateError error
    )
        : this(new[] { error })
    {
    }

    public Institution? Institution { get; }
    public IReadOnlyCollection<ChangeInstitutionOperatingStateError>? Errors { get; }
}