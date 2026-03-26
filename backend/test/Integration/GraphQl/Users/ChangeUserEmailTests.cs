using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ChangeUserEmailTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidData_EmailsConfirmationCode()
    {
        // Arrange
        const string name = "John Doe";
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterAndConfirmAndLoginUser();
        EmailSender.Clear();
        const string newEmail = "new." + email;
        // Act
        var response = await ChangeUserEmail(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            newEmail
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
        EmailsShouldContainSingle(
            (name, newEmail),
            "Confirm your email change",
            @"^Please confirm your email address change by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email-change\?currentEmail=john\.doe@ise\.fraunhofer\.de&newEmail=new.john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NonLoggedInUser_IsAuthenticationError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmUser(
            email: email,
            password: password
        );
        const string newEmail = "new." + email;
        // Act
        var response = await QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ChangeUserEmail.graphql"),
            new Dictionary<string, object?>
            {
                ["newEmail"] = newEmail
            },
            HttpSuccess,
            AsString,
            ForSnapshotMatch
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnchangedEmail_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserEmail(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidEmail_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        const string newEmail = "@invalid@" + email;
        // Act
        var response = await ChangeUserEmail(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            newEmail
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }
}