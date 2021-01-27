using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class LoginUserPayload
      : UserPayload<LoginUserError>
    {
        // TODO Use proper type like https://docs.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytoken?view=azure-dotnet
        public string? JwtAccessToken { get; }

        public bool? RequiresTwoFactor { get; }

        public LoginUserPayload(
            string jwtAccessToken,
            Data.User user
            )
          : base(user)
        {
            JwtAccessToken = jwtAccessToken;
            RequiresTwoFactor = false;
        }

        public LoginUserPayload(
            Data.User user
            )
          : base(user)
        {
            RequiresTwoFactor = true;
        }

        public LoginUserPayload(
            LoginUserError error
            )
          : base(error)
        {
        }
    }
}