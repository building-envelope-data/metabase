namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionOperatedDatabaseEdge
    {
        public Data.Database Node { get; }

        public InstitutionOperatedDatabaseEdge(
            Data.Database node
        )
        {
            Node = node;
        }
    }
}