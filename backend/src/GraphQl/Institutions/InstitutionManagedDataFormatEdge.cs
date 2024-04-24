using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedDataFormatEdge
{
    public InstitutionManagedDataFormatEdge(
        DataFormat node
    )
    {
        Node = node;
    }

    public DataFormat Node { get; }
}