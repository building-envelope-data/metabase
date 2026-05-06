using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseOperatorEdge(
    Database association
    )
        : Edge<Institution, IInstitutionByIdDataLoader>(association.OperatorId)
{
}