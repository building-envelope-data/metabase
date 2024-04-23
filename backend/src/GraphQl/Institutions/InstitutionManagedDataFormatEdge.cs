namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedDataFormatEdge
{
    public Data.DataFormat Node { get; }

    public InstitutionManagedDataFormatEdge(
        Data.DataFormat node
    )
    {
        Node = node;
    }
}