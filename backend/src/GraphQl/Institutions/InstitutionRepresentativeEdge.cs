using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeEdge
    : Edge<User, UserByIdDataLoader>
{
    public InstitutionRepresentativeEdge(
        InstitutionRepresentative association
    )
        : base(association.UserId)
    {
        Role = association.Role;
    }

    public InstitutionRepresentativeRole Role { get; }
}