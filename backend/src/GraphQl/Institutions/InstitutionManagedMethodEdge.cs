using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed record InstitutionManagedMethodEdge(
    Method Node,
    string Cursor
)
: PaginatedEdge<Method>(
    Node,
    Cursor
);