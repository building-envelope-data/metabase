using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodManagerEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public MethodManagerEdge(
            Data.Method association
        )
            : base(association.ManagerId)
        {
        }
    }
}