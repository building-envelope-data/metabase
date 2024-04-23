using System.Collections.Generic;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed class AddInstitutionRepresentativePayload
{
    public UserRepresentedInstitutionEdge? RepresentedInstitutionEdge { get; }
    public InstitutionRepresentativeEdge? InstitutionRepresentativeEdge { get; }
    public IReadOnlyCollection<AddInstitutionRepresentativeError>? Errors { get; }

    public AddInstitutionRepresentativePayload(
        Data.InstitutionRepresentative institutionRepresentative
    )
    {
        RepresentedInstitutionEdge = new UserRepresentedInstitutionEdge(institutionRepresentative);
        InstitutionRepresentativeEdge = new InstitutionRepresentativeEdge(institutionRepresentative);
    }

    public AddInstitutionRepresentativePayload(
        IReadOnlyCollection<AddInstitutionRepresentativeError> errors
    )
    {
        Errors = errors;
    }

    public AddInstitutionRepresentativePayload(
        AddInstitutionRepresentativeError error
    )
        : this(new[] { error })
    {
    }
}