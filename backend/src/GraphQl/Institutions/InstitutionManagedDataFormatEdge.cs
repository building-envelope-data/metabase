using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed record InstitutionManagedDataFormatEdge(
    DataFormat Node,
    string Cursor
)
: PaginatedEdge<DataFormat>(
    Node,
    Cursor
);