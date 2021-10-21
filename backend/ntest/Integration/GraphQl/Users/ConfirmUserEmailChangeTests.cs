using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.NTests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class ConfirmUserEmailChangeNTests
      : UserIntegrationNTests
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
        public async Task ValidData_ConfirmsUserEmailChange()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var newEmail = "new." + email;
            var confirmationCode = await Arrange(
                email: email,
                newEmail: newEmail
            ).ConfigureAwait(false);
            // Act
            var response = await ConfirmUserEmailChange(
                currentEmail: email,
                newEmail: newEmail,
                confirmationCode: confirmationCode
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
        public async Task UnknownUser_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var newEmail = "new." + email;
            var confirmationCode = await Arrange(
                email: email,
                newEmail: newEmail
            ).ConfigureAwait(false);
            // Act
            var response = await ConfirmUserEmailChange(
                currentEmail: "unknown." + email,
                newEmail: newEmail,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task DuplicateEmail_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var newEmail = "new." + email;
            var confirmationCode = await Arrange(
                email: email,
                newEmail: newEmail
            ).ConfigureAwait(false);
            await RegisterAndConfirmUser(
                email: newEmail,
                password: DefaultPassword
                ).ConfigureAwait(false);
            // Act
            var response = await ConfirmUserEmailChange(
                currentEmail: email,
                newEmail: newEmail,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task DifferentNewEmail_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var newEmail = "new." + email;
            var confirmationCode = await Arrange(
                email: email,
                newEmail: newEmail
            ).ConfigureAwait(false);
            // Act
            var response = await ConfirmUserEmailChange(
                currentEmail: email,
                newEmail: "other." + newEmail,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task InvalidConfirmationCode_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var newEmail = "new." + email;
            var confirmationCode = await Arrange(
                email: email,
                newEmail: newEmail
            ).ConfigureAwait(false);
            // Act
            var response = await ConfirmUserEmailChange(
                currentEmail: email,
                newEmail: newEmail,
                confirmationCode: "invalid" + confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}