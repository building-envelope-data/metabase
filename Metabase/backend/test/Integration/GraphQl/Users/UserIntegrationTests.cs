using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public abstract class UserIntegrationTests
      : IntegrationTests
    {
        protected Task<string> ChangeUserPassword(
            string currentPassword,
            string newPassword,
            string? newPasswordConfirmation = null
            )
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ChangeUserPassword.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["currentPassword"] = currentPassword,
                    ["newPassword"] = newPassword,
                    ["newPasswordConfirmation"] = newPasswordConfirmation ?? newPassword
                }
                );
        }

        protected Task<string> ResendUserEmailConfirmation(
            string email
            )
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ResendUserEmailConfirmation.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["email"] = email
                }
                );
        }

        protected Task<string> RequestUserPasswordReset(
            string email
            )
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RequestUserPasswordReset.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["email"] = email
                }
                );
        }

        protected Task<string> ResetUserPassword(
            string email,
            string password,
            string resetCode,
            string? passwordConfirmation = null
            )
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ResetUserPassword.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["email"] = email,
                    ["password"] = password,
                    ["passwordConfirmation"] = passwordConfirmation ?? password,
                    ["resetCode"] = resetCode
                }
                );
        }
    }
}