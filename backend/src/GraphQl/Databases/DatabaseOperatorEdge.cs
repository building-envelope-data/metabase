using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseOperatorEdge
    : Edge<Institution, InstitutionByIdDataLoader>
{
    public DatabaseOperatorEdge(
        Database association
    )
        : base(association.OperatorId)
    {
    }
}