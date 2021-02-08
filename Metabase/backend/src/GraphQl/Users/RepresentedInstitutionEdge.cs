using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users
{
    public sealed class RepresentedInstitutionEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public Enumerations.InstitutionRepresentativeRole Role { get; }

        public RepresentedInstitutionEdge(
            Data.InstitutionRepresentative institutionRepresentative
        )
            : base(institutionRepresentative.UserId)
        {
            Role = institutionRepresentative.Role;
        }
    }
}