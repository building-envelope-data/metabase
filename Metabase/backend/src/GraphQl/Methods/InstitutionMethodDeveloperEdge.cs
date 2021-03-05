using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Methods
{
    public sealed class InstitutionMethodDeveloperEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public InstitutionMethodDeveloperEdge(
            Data.InstitutionMethodDeveloper association
        )
            : base(association.InstitutionId)
        {
        }
    }
}