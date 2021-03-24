namespace Metabase.GraphQl.Users
{
    public sealed class ForgetUserTwoFactorAuthenticationClientPayload
      : UserPayload<ForgetUserTwoFactorAuthenticationClientError>
    {
        public ForgetUserTwoFactorAuthenticationClientPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public ForgetUserTwoFactorAuthenticationClientPayload(
            ForgetUserTwoFactorAuthenticationClientError error
            )
          : base(error)
        {
        }
    }
}