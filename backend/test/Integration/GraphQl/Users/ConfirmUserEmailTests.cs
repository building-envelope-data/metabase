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
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            confirmationCode
        );
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
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        EmailSender.Clear();
        await ResendUserEmailConfirmation(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            confirmationCode
        );
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
        await ResendUserEmailVerification(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            confirmationCode
        );
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
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        EmailSender.Clear();
        // Act
        await ResendUserEmailConfirmation(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email
        );
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            ExtractConfirmationCodeFromEmail()
        );
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
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            confirmationCode,
            "unknown." + email
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidConfirmationCode_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        // Act
        var response = await ConfirmUserEmail(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            "invalid" + confirmationCode
        );
        // Assert
        Snapshot.Match(response);
    }
}