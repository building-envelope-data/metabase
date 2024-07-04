using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodEdge
    : Edge<Method, MethodByIdDataLoader>
{
    public UserDevelopedMethodEdge(
        UserMethodDeveloper association
    )
        : base(association.MethodId)
    {
    }
}