using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedComponentEdge(
    Component node
    )
{
    public Component Node { get; } = node;
}