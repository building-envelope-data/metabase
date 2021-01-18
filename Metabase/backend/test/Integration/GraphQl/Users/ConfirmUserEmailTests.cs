using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using System.Collections.Generic;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class ConfirmUserEmailTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ValidData_ConfirmsUserEmail()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromRegistrationEmail();
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
        public async Task UnknownUser_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromRegistrationEmail();
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
            var confirmationCode = ExtractConfirmationCodeFromRegistrationEmail();
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
