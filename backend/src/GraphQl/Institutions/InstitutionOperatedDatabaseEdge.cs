using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabaseEdge
{
    public InstitutionOperatedDatabaseEdge(
        Database node
    )
    {
        Node = node;
    }

    public Database Node { get; }
}