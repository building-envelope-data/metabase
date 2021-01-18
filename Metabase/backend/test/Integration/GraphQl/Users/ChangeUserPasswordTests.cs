using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using System.Collections.Generic;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class ChangeUserPasswordTests
      : UserIntegrationTests
    {
        private async Task<string> ChangeUserPassword(
            string currentPassword,
            string newPassword,
            string? newPasswordConfirmation = null
            )
        {
            return await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ChangeUserPassword.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["currentPassword"] = currentPassword,
                    ["newPassword"] = newPassword,
                    ["newPasswordConfirmation"] = newPasswordConfirmation ?? newPassword
                }
                ).ConfigureAwait(false);
        }

        [Fact]
        public async Task ValidDataOfRegisteredAndConfirmedAndLoggedInUser_ChangesUserPassword()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await ChangeUserPassword(
                currentPassword: password,
                newPassword: "new" + password
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        // TODO Other cases!
    }
}
