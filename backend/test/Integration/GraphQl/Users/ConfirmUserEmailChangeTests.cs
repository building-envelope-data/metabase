using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ConfirmUserEmailChangeTests
    : UserIntegrationTests
{
    private async Task<string> Arrange(
        string email,
        string newEmail
    )
    {
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: DefaultPassword
        ).ConfigureAwait(false);
        EmailSender.Clear();
        await ChangeUserEmail(
            newEmail
        ).ConfigureAwait(false);
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        EmailSender.Clear();
        return confirmationCode;
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidData_ConfirmsUserEmailChange()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        ).ConfigureAwait(false);
        // Act
        var response = await ConfirmUserEmailChange(
            email,
            newEmail,
            confirmationCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.confirmUserEmailChange.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownUser_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        ).ConfigureAwait(false);
        // Act
        var response = await ConfirmUserEmailChange(
            "unknown." + email,
            newEmail,
            confirmationCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task DuplicateEmail_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        ).ConfigureAwait(false);
        await RegisterAndConfirmUser(
            email: newEmail,
            password: DefaultPassword
        ).ConfigureAwait(false);
        // Act
        var response = await ConfirmUserEmailChange(
            email,
            newEmail,
            confirmationCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task DifferentNewEmail_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        ).ConfigureAwait(false);
        // Act
        var response = await ConfirmUserEmailChange(
            email,
            "other." + newEmail,
            confirmationCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidConfirmationCode_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        ).ConfigureAwait(false);
        // Act
        var response = await ConfirmUserEmailChange(
            email,
            newEmail,
            "invalid" + confirmationCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
    }
}