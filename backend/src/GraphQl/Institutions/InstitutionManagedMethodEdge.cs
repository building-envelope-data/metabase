using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedMethodEdge
{
    public InstitutionManagedMethodEdge(
        Method node
    )
    {
        Node = node;
    }

    public Method Node { get; }
}