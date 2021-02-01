using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public ComponentManufacturerEdge(
            Data.ComponentManufacturer association
        )
            : base(association.InstitutionId)
        {
        }
    }
}