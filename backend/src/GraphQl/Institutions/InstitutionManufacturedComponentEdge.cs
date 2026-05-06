using Metabase.Data;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentEdge(
    ComponentManufacturer association,
    string cursor
)
: PaginatedEdge<Component, IComponentByIdDataLoader>(
    association.ComponentId,
    cursor
)
{
}