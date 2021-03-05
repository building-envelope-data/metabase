namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedDataFormatConnection
        : Connection<Data.Institution, Data.DataFormat, InstitutionManagedDataFormatsByInstitutionIdDataLoader, InstitutionManagedDataFormatEdge>
    {
        public InstitutionManagedDataFormatConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManagedDataFormatEdge(x)
                )
        {
        }
    }
}