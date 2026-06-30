using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed record InstitutionManagedComponentEdge(
    Component Node,
    string Cursor
)
: PaginatedEdge<Component>(
    Node,
    Cursor
);