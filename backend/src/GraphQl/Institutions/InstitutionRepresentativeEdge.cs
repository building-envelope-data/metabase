using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeEdge
    : Edge<Data.User, UserByIdDataLoader>
{
    public Enumerations.InstitutionRepresentativeRole Role { get; }

    public InstitutionRepresentativeEdge(
        Data.InstitutionRepresentative association
    )
        : base(association.UserId)
    {
        Role = association.Role;
    }
}