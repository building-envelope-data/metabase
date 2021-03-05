using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users
{
    public sealed class UserRepresentedInstitutionEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public UserRepresentedInstitutionEdge(
            Data.InstitutionRepresentative association
        )
            : base(association.InstitutionId)
        {
        }
    }
}