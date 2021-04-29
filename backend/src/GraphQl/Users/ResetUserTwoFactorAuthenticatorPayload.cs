namespace Metabase.GraphQl.Users
{
    public sealed class ResetUserTwoFactorAuthenticatorPayload
      : UserPayload<ResetUserTwoFactorAuthenticatorError>
    {
        public ResetUserTwoFactorAuthenticatorPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public ResetUserTwoFactorAuthenticatorPayload(
            ResetUserTwoFactorAuthenticatorError error
            )
          : base(error)
        {
        }
    }
}