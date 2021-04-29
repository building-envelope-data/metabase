using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public InstitutionManufacturedComponentEdge(
            Data.ComponentManufacturer association
        )
            : base(association.ComponentId)
        {
        }
    }
}