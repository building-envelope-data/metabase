using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NUnit.Framework;
using Snapshooter.NUnit;

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
        );
        EmailSender.Clear();
        await RequestUserPasswordReset(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email
        );
        return ExtractResetCodeFromEmail();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidData_ResetsUserPassword()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        const string NewPassword = "new" + Password;
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            NewPassword,
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser(
            Email,
            NewPassword
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NonBase64ResetCode_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "new" + Password,
            "nonBase64" + resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task InvalidResetCode_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "new" + Password,
            "SSBhbSBhIGZha2UgYmFzZTY0IGVuY29kZWQgdG9rZW4="
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordConfirmationMismatch_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "new" + Password,
            resetCode,
            "other" + Password);
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresDigit_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "aabb@$CCDD",
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresLower_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "AABB@$567",
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresNonAlphanumeric_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "aaBBccDDeeFF123",
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresUpper_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "aabb@$567",
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordTooShort_IsUserError()
    {
        // Arrange
        const string Email = "john.doe@ise.fraunhofer.de";
        const string Password = "aaaAAA123$!@";
        var resetCode = await RegisterAndConfirmUserAndRequestPasswordReset(
            Email,
            Password
        );
        // Act
        var response = await ResetUserPassword(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            Email,
            "aA@$567",
            resetCode
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }
}