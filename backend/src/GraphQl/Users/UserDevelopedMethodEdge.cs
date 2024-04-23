using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodEdge
    : Edge<Data.Method, MethodByIdDataLoader>
{
    public UserDevelopedMethodEdge(
        Data.UserMethodDeveloper association
    )
        : base(association.MethodId)
    {
    }
}