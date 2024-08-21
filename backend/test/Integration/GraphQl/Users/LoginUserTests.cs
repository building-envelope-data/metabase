using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class LoginUserTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidDataOfRegisteredAndConfirmedUser_LogsInUser()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmUser(email: email, password: password).ConfigureAwait(false);
        // Act
        var response = await SuccessfullyQueryGraphQlContentAsString(
            File.ReadAllText("Integration/GraphQl/Users/LoginUser.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["email"] = email,
                ["password"] = password,
            }
        ).ConfigureAwait(false);
        // Assert
        // TODO assert that cookie was set!
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.loginUser.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    // TODO Other cases!
}