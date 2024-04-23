using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedInstitutionEdge
{
    public InstitutionManagedInstitutionEdge(
        Institution node
    )
    {
        Node = node;
    }

    public Institution Node { get; }
}