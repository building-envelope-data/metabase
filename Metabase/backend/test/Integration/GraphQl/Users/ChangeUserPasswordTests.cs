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
        [Fact]
        public async Task ValidDataOfRegisteredAndConfirmedAndLoggedInUser_ChangesUserPassword()
        {
            // Arrange
            var email = DefaultEmail;
            var password = DefaultPassword;
            await RegisterAndConfirmAndAuthorizeUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ChangeUserPasswordTests/ValidDataOfRegisteredAndConfirmedAndLoggedInUser_ChangesUserPassword.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["currentPassword"] = password,
                    ["newPassword"] = "new" + password,
                    ["newPasswordConfirmation"] = "new" + password
                }
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
