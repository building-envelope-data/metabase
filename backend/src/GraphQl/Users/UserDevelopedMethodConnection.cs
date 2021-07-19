namespace Metabase.GraphQl.Users
{
    public sealed class UserDevelopedMethodConnection
        : ForkingConnection<Data.User, Data.UserMethodDeveloper, PendingUserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge>
    {
        public UserDevelopedMethodConnection(
            Data.User subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new UserDevelopedMethodEdge(x)
                )
        {
        }
    }
}