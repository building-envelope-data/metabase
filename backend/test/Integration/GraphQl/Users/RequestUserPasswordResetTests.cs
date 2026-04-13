using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class RequestUserPasswordResetTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ExistingAndConfirmedEmailAddress_RequestsUserPasswordReset()
    {
        // Arrange
        const string name = "John Doe";
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterAndConfirmUser();
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            email
        );
        // Assert
        Snapshot.Match(response);
        EmailsShouldContainSingle(
            (name, email),
            "Reset password",
            $@"^{Regex.Escape($"Please reset your password by following the link {AppSettings.Uri.AbsoluteUri}users/reset-password?resetCode=")}\w+$"
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownEmailAddress_DoesNothing()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterAndConfirmUser(email: email);
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            "unknown." + email
        );
        // Assert
        Snapshot.Match(response);
        EmailSender.Emails.Should().BeEmpty();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnconfirmedEmailAddress_DoesNothing()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            email
        );
        // Assert
        Snapshot.Match(response);
        EmailSender.Emails.Should().BeEmpty();
    }
}