using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods
{
    public sealed class UserMethodDeveloperEdge
        : Edge<Data.User, UserByIdDataLoader>
    {
        public UserMethodDeveloperEdge(
            Data.UserMethodDeveloper association
        )
            : base(association.UserId)
        {
        }
    }
}