using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ConfirmUserEmailTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidDataWithConfirmationCodeFromRegistrationEmail_ConfirmsUserEmail()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(email: email);
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            confirmationCode);
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidDataWithConfirmationCodeFromResendUserEmailConfirmation_ConfirmsUserEmail()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(email: email);
        EmailSender.Clear();
        await ResendUserEmailConfirmation(
            email
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            confirmationCode);
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidDataWithConfirmationCodeFromResendUserEmailVerification_ConfirmsUserEmail()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterAndConfirmAndLoginUser(email: email);
        EmailSender.Clear();
        await ResendUserEmailVerification();
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            confirmationCode);
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ResentUserEmailConfirmation_ContainsValidCode()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(email: email);
        EmailSender.Clear();
        // Act
        await ResendUserEmailConfirmation(
            email
        );
        var response = await ConfirmUserEmail(
            ExtractConfirmationCodeFromEmail());
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownUser_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(email: email);
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            confirmationCode,
            "unknown." + email);
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidConfirmationCode_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(email: email);
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            "invalid" + confirmationCode);
        // Assert
        Snapshot.Match(response);
    }
}