using FluentAssertions;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [Collection(nameof(Data.User))]
    public sealed class ConfirmUserEmailTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ValidDataWithConfirmationCodeFromRegistrationEmail_ConfirmsUserEmail()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                email: email,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Fact]
        public async Task ValidDataWithConfirmationCodeFromResendUserEmailConfirmation_ConfirmsUserEmail()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                email: email,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Fact]
        public async Task ValidDataWithConfirmationCodeFromResendUserEmailVerification_ConfirmsUserEmail()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterAndConfirmAndLoginUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            await ResendUserEmailVerification().ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                email: email,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Fact]
        public async Task ResentUserEmailConfirmation_ContainsValidCode()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            var response = await ConfirmUserEmail(
                email: email,
                confirmationCode: ExtractConfirmationCodeFromEmail()
            ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.confirmUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
            );
        }


        [Fact]
        public async Task UnknownUser_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                email: "unknown." + email,
                confirmationCode: confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task InvalidConfirmationCode_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            // Act
            var response = await ConfirmUserEmail(
                email: email,
                confirmationCode: "invalid" + confirmationCode
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}
