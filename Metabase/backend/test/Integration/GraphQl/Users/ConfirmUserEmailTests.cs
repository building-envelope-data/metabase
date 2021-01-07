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
            var confirmationCode = await RegisterUser();
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailTests/ValidData_ConfirmsUserEmail.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["confirmationCode"] = confirmationCode
                }
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
            var confirmationCode = await RegisterUser();
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailTests/UnknownUser_IsUserError.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["confirmationCode"] = confirmationCode
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task InvalidConfirmationCode_IsUserError()
        {
            // Arrange
            var confirmationCode = await RegisterUser();
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailTests/InvalidConfirmationCode_IsUserError.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["confirmationCode"] = "invalid" + confirmationCode
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}
