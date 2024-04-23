namespace Metabase.GraphQl.Users
{
    public sealed class LoginUserWithRecoveryCodePayload
        : UserPayload<LoginUserWithRecoveryCodeError>
    {
        public LoginUserWithRecoveryCodePayload(
            Data.User user
        )
            : base(user)
        {
        }

        public LoginUserWithRecoveryCodePayload(
            LoginUserWithRecoveryCodeError error
        )
            : base(error)
        {
        }
    }
}