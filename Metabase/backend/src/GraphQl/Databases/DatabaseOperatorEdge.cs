using HotChocolate.Types;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseOperatorEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public DatabaseOperatorEdge(
            Data.Database association
        )
            : base(association.OperatorId)
        {
        }
    }

    public sealed class DatabaseOperatorEdgeType
      : ObjectType<DatabaseOperatorEdge>
    {
    }
}