namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentConnection
        : Connection<Data.Institution, Data.ComponentManufacturer, InstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentEdge>
    {
        public InstitutionManufacturedComponentConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManufacturedComponentEdge(x)
                )
        {
        }
    }
}