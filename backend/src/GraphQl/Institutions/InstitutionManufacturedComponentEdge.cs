using Metabase.Data;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentEdge
    : Edge<Component, ComponentByIdDataLoader>
{
    public InstitutionManufacturedComponentEdge(
        ComponentManufacturer association
    )
        : base(association.ComponentId)
    {
    }
}