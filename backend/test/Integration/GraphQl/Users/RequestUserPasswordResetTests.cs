using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

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
        await RegisterAndConfirmUser(
            name,
            email
        ).ConfigureAwait(false);
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            email
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        EmailsShouldContainSingle(
            (name, email),
            "Reset password",
            @"^Please reset your password by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/reset-password\?resetCode=\w+\.$"
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownEmailAddress_DoesNothing()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterAndConfirmUser(email: email).ConfigureAwait(false);
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            "unknown." + email
        ).ConfigureAwait(false);
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
        await RegisterUser(email: email).ConfigureAwait(false);
        EmailSender.Clear();
        // Act
        var response = await RequestUserPasswordReset(
            email
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        EmailSender.Emails.Should().BeEmpty();
    }
}