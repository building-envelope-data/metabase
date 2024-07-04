using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration.GraphQl.Users;

public abstract class UserIntegrationTests
    : IntegrationTests
{
    protected Task<string> GetUser(Guid uuid)
    {
        return SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/GetUser.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["uuid"] = uuid
            }
        );
    }

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

    protected Task<string> DeletePersonalUserData(
        string? password
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/DeletePersonalUserData.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["password"] = password
            }
        );
    }

    protected Task<string> ChangeUserEmail(
        string newEmail
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/ChangeUserEmail.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["newEmail"] = newEmail
            }
        );
    }

    protected Task<string> ConfirmUserEmailChange(
        string currentEmail,
        string newEmail,
        string confirmationCode
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailChange.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["currentEmail"] = currentEmail,
                ["newEmail"] = newEmail,
                ["confirmationCode"] = confirmationCode
            }
        );
    }

    protected Task<string> ResendUserEmailVerification()
    {
        return SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/ResendUserEmailVerification.graphql")
        );
    }
}