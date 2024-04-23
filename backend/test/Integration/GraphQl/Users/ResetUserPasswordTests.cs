using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ResetUserPasswordTests
    : UserIntegrationTests
{
    private async Task<string> RegisterAndConfirmUserAndRequestPasswordReset(
        string email,
        string password
    )
    {
        await RegisterAndConfirmUser(
            email: email,
            password: password
        ).ConfigureAwait(false);
        EmailSender.Clear();
        await RequestUserPasswordReset(
            email
        ).ConfigureAwait(false);
        return ExtractResetCodeFromEmail();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidData_ResetsUserPassword()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        const string newPassword = "new" + password;
        // Act
        var response = await ResetUserPassword(
            email,
            newPassword,
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            newPassword
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidResetCode_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "new" + password,
            "invalid" + resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordConfirmationMismatch_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "new" + password,
            resetCode,
            "other" + password).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresDigit_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "aabb@$CCDD",
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresLower_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "AABB@$567",
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresNonAlphanumeric_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "aaBBccDDeeFF123",
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresUpper_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "aabb@$567",
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordTooShort_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            email,
            password
        ).ConfigureAwait(false);
        // Act
        var response = await ResetUserPassword(
            email,
            "aA@$567",
            resetCode
        ).ConfigureAwait(false);
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            email,
            password
        ).ConfigureAwait(false);
    }
}