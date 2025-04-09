using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public class GrantPermissionToSignDataPayload
{
    public GrantPermissionToSignDataPayload(
        InstitutionRepresentative institutionRepresentative
    )
    {
        InstitutionRepresentative = institutionRepresentative;
    }

    public GrantPermissionToSignDataPayload(
        IReadOnlyCollection<GrantPermissionToSignDataError> errors
    )
    {
        Errors = errors;
    }

    public GrantPermissionToSignDataPayload(
        InstitutionRepresentative institutionRepresentative,
        IReadOnlyCollection<GrantPermissionToSignDataError> errors
    )
    {
        InstitutionRepresentative = institutionRepresentative;
        Errors = errors;
    }

    public GrantPermissionToSignDataPayload(
        GrantPermissionToSignDataError error
    )
        : this([error])
    {
    }

    public InstitutionRepresentative? InstitutionRepresentative { get; }
    public IReadOnlyCollection<GrantPermissionToSignDataError>? Errors { get; }
}