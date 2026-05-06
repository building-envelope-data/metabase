using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodEdge(
    InstitutionMethodDeveloper association,
    string cursor
)
: PaginatedEdge<Method, IMethodByIdDataLoader>(
    association.MethodId, cursor
)
{
}