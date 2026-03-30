using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodEdge(
    UserMethodDeveloper association,
    string cursor
)
: PaginatedEdge<Method, IMethodByIdDataLoader>(
    association.MethodId,
    cursor
)
{
}