namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionDevelopedMethodConnection
        : ForkingConnection<Data.Institution, Data.InstitutionMethodDeveloper, PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>
    {
        public InstitutionDevelopedMethodConnection(
            Data.Institution institution,
            bool pending
        )
            : base(
                institution,
                pending,
                x => new InstitutionDevelopedMethodEdge(x)
                )
        {
        }
    }
}