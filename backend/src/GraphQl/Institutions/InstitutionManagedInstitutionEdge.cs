namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedInstitutionEdge
    {
        public Data.Institution Node { get; }

        public InstitutionManagedInstitutionEdge(
            Data.Institution node
        )
        {
            Node = node;
        }
    }
}