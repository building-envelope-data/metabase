using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class LoginUserPayload
    : UserPayload
    {
        // TODO Use proper type like https://docs.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytoken?view=azure-dotnet
        public string? JwtAccessToken { get; }

        public IReadOnlyCollection<LoginUserError>? Errors { get; }

        public LoginUserPayload(
            string jwtAccessToken,
            Data.User user
            )
          : base(user)
        {
            JwtAccessToken = jwtAccessToken;
        }

        public LoginUserPayload(
            IReadOnlyCollection<LoginUserError> errors
            )
        {
            Errors = errors;
        }

        public LoginUserPayload(
            LoginUserError error
            )
        {
          Errors = new [] { error };
        }
    }
}
