using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration.GraphQl.Users;

public abstract class UserIntegrationTests
    : IntegrationTests
{
    protected Task<T> GetUser<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        Guid id
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/GetUser.graphql"),
            new Dictionary<string, object?>
            {
                ["id"] = id
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ChangeUserPassword<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string currentPassword,
        string newPassword,
        string? newPasswordConfirmation = null
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ChangeUserPassword.graphql"),
            new Dictionary<string, object?>
            {
                ["currentPassword"] = currentPassword,
                ["newPassword"] = newPassword,
                ["newPasswordConfirmation"] = newPasswordConfirmation ?? newPassword
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ResendUserEmailConfirmation<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string email
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ResendUserEmailConfirmation.graphql"),
            new Dictionary<string, object?>
            {
                ["email"] = email
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> RequestUserPasswordReset<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string email
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/RequestUserPasswordReset.graphql"),
            new Dictionary<string, object?>
            {
                ["email"] = email
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ResetUserPassword<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string email,
        string password,
        string resetCode,
        string? passwordConfirmation = null
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ResetUserPassword.graphql"),
            new Dictionary<string, object?>
            {
                ["email"] = email,
                ["password"] = password,
                ["passwordConfirmation"] = passwordConfirmation ?? password,
                ["resetCode"] = resetCode
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> DeletePersonalUserData<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string? password
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/DeletePersonalUserData.graphql"),
            new Dictionary<string, object?>
            {
                ["password"] = password
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ChangeUserEmail<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string newEmail
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ChangeUserEmail.graphql"),
            new Dictionary<string, object?>
            {
                ["newEmail"] = newEmail
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ConfirmUserEmailChange<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string currentEmail,
        string newEmail,
        string confirmationCode
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailChange.graphql"),
            new Dictionary<string, object?>
            {
                ["currentEmail"] = currentEmail,
                ["newEmail"] = newEmail,
                ["confirmationCode"] = confirmationCode
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> ResendUserEmailVerification<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ResendUserEmailVerification.graphql"),
            null,
            assertBefore,
            read,
            assertAfter
        );
    }
}
