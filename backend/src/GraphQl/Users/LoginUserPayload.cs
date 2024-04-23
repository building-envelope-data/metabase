namespace Metabase.GraphQl.Users
{
    public sealed class LoginUserPayload
        : UserPayload<LoginUserError>
    {
        public bool? RequiresTwoFactor { get; }

        public LoginUserPayload(
            Data.User user,
            bool requiresTwoFactor
        )
            : base(user)
        {
            RequiresTwoFactor = requiresTwoFactor;
        }

        public LoginUserPayload(
            LoginUserError error
        )
            : base(error)
        {
        }
    }
}