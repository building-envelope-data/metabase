namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionRepresentativeConnection
        : Connection<Data.Institution, Data.InstitutionRepresentative, InstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativeEdge>
    {
        public InstitutionRepresentativeConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionRepresentativeEdge(x)
                )
        {
        }
    }
}