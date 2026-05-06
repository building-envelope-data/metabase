using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed record InstitutionManagedInstitutionEdge(
    Institution Node,
    string Cursor
)
: PaginatedEdge<Institution>(
    Node,
    Cursor
);