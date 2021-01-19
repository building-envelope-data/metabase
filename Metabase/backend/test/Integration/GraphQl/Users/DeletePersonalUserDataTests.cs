using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using System.Collections.Generic;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class DeletePersonalUserDataTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task CorrectPassword_IsSuccess()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await DeletePersonalUserData(
                password: password
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Fact]
        public async Task CorrectPassword_DeletesPersonalUserData()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                password: password
                ).ConfigureAwait(false);
            var response = await GetPersonalUserData().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task NonLoggedInUser_IsAuthenticationError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/DeletePersonalUserData.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["password"] = password
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task NonLoggedInUser_DoesNotDeletePersonalUserData()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/DeletePersonalUserData.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["password"] = password
                }
                ).ConfigureAwait(false);
            var response = await GetPersonalUserData().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task MissingPassword_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await DeletePersonalUserData(
                null
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task MissingPassword_DoesNotDeletePersonalUserData()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                null
                ).ConfigureAwait(false);
            var response = await GetPersonalUserData().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task IncorrectPassword_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await DeletePersonalUserData(
                "incorrect" + password
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task IncorrectPassword_DoesNotDeletePersonalUserData()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                "incorrect" + password
                ).ConfigureAwait(false);
            var response = await GetPersonalUserData().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}
