using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManagerEdge(
    Component association
    )
        : Edge<Institution, InstitutionByIdDataLoader>(association.ManagerId)
{
}
