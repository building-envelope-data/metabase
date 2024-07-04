using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Methods;

public sealed class MethodManagerEdge
    : Edge<Institution, InstitutionByIdDataLoader>
{
    public MethodManagerEdge(
        Method association
    )
        : base(association.ManagerId)
    {
    }
}