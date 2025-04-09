using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public class ForbidPermissionToSignDataPayload
{
    public ForbidPermissionToSignDataPayload(
        InstitutionRepresentative institutionRepresentative
    )
    {
        InstitutionRepresentative = institutionRepresentative;
    }

    public ForbidPermissionToSignDataPayload(
        IReadOnlyCollection<ForbidPermissionToSignDataError> errors
    )
    {
        Errors = errors;
    }

    public ForbidPermissionToSignDataPayload(
        InstitutionRepresentative institutionRepresentative,
        IReadOnlyCollection<ForbidPermissionToSignDataError> errors
    )
    {
        InstitutionRepresentative = institutionRepresentative;
        Errors = errors;
    }

    public ForbidPermissionToSignDataPayload(
        ForbidPermissionToSignDataError error
    )
        : this([error])
    {
    }

    public InstitutionRepresentative? InstitutionRepresentative { get; }
    public IReadOnlyCollection<ForbidPermissionToSignDataError>? Errors { get; }
}