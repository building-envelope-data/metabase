using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            const string newPassword = "new" + password;
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: newPassword,
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: newPassword
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "new" + password,
                resetCode: "invalid" + resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "new" + password,
                resetCode: resetCode,
                passwordConfirmation: "other" + password).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "aabb@$CCDD",
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "AABB@$567",
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "aaBBccDDeeFF123",
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "aabb@$567",
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
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
                email: email,
                password: password
                ).ConfigureAwait(false);
            // Act
            var response = await ResetUserPassword(
                email: email,
                password: "aA@$567",
                resetCode: resetCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }
    }
}
