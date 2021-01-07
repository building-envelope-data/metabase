using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class LoginUserTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ValidDataOfRegisteredAndConfirmedUser_LogsInUser()
        {
            // Arrange
            await ConfirmUserEmail(
                await RegisterUser().ConfigureAwait(false)
                ).ConfigureAwait(false);
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/LoginUserTests/ValidDataOfRegisteredAndConfirmedUser_LogsInUser.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.loginUser.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        // TODO Other cases!
    }
}
