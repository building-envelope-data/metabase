using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionEdge
    : Edge<Institution, InstitutionByIdDataLoader>
{
    public UserRepresentedInstitutionEdge(
        InstitutionRepresentative association
    )
        : base(association.InstitutionId)
    {
        Role = association.Role;
    }

    public InstitutionRepresentativeRole Role { get; }
}