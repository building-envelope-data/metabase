using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

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
        );
        EmailSender.Clear();
        await ChangeUserEmail(
            HttpSuccess,
            AsJson,
            NoGraphQlErrors,
            newEmail
        );
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
        );
        // Act
        var response = await ConfirmUserEmailChange(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email,
            newEmail,
            confirmationCode
        );
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
        );
        // Act
        var response = await ConfirmUserEmailChange(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            "unknown." + email,
            newEmail,
            confirmationCode
        );
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
        );
        await RegisterAndConfirmUser(
            email: newEmail,
            password: DefaultPassword
        );
        // Act
        var response = await ConfirmUserEmailChange(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email,
            newEmail,
            confirmationCode
        );
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
        );
        // Act
        var response = await ConfirmUserEmailChange(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email,
            "other." + newEmail,
            confirmationCode
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
        const string newEmail = "new." + email;
        var confirmationCode = await Arrange(
            email,
            newEmail
        );
        // Act
        var response = await ConfirmUserEmailChange(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email,
            newEmail,
            "invalid" + confirmationCode
        );
        // Assert
        Snapshot.Match(response);
    }
}