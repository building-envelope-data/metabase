namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionOperatedDatabaseConnection
        : Connection<Data.Institution, Data.Database, InstitutionOperatedDatabasesByInstitutionIdDataLoader, InstitutionOperatedDatabaseEdge>
    {
        public InstitutionOperatedDatabaseConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionOperatedDatabaseEdge(x)
                )
        {
        }
    }
}