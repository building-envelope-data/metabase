using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionEdge
    : Edge<Data.Institution, InstitutionByIdDataLoader>
{
    public Enumerations.InstitutionRepresentativeRole Role { get; }

    public UserRepresentedInstitutionEdge(
        Data.InstitutionRepresentative association
    )
        : base(association.InstitutionId)
    {
        Role = association.Role;
    }
}