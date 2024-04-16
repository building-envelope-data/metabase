using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
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
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                confirmationCode: confirmationCode
,
                email: email).ConfigureAwait(false);
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
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                confirmationCode: confirmationCode
,
                email: email).ConfigureAwait(false);
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
            await RegisterAndConfirmAndLoginUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            await ResendUserEmailVerification().ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                confirmationCode: confirmationCode
,
                email: email).ConfigureAwait(false);
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
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            var response = await ConfirmUserEmail(
                confirmationCode: ExtractConfirmationCodeFromEmail()
,
                email: email).ConfigureAwait(false);
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
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                confirmationCode: confirmationCode,
                email: "unknown." + email).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task InvalidConfirmationCode_IsUserError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                confirmationCode: "invalid" + confirmationCode,
                email: email).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}
