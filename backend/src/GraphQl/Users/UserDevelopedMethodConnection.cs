namespace Metabase.GraphQl.Users
{
    public sealed class UserDevelopedMethodConnection
        : Connection<Data.User, Data.UserMethodDeveloper, UserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge>
    {
        public UserDevelopedMethodConnection(
            Data.User subject
        )
            : base(
                subject,
                x => new UserDevelopedMethodEdge(x)
                )
        {
        }
    }
}