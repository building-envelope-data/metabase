namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionDevelopedMethodConnection
        : Connection<Data.Institution, Data.InstitutionMethodDeveloper, InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>
    {
        public InstitutionDevelopedMethodConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionDevelopedMethodEdge(x)
                )
        {
        }
    }
}