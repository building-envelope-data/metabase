namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedMethodEdge
    {
        public Data.Method Node { get; }

        public InstitutionManagedMethodEdge(
            Data.Method node
        )
        {
            Node = node;
        }
    }
}