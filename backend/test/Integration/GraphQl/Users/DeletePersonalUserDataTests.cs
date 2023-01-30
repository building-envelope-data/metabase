using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class DeletePersonalUserDataTests
      : UserIntegrationTests
    {
        [Test]
        public async Task CorrectPassword_IsSuccess()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
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

        [Test]
        public async Task CorrectPassword_DeletesPersonalUserData()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            var userId =
                await RegisterAndConfirmAndLoginUser(
                    email: email,
                    password: password
                ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                password: password
                ).ConfigureAwait(false);
            var response = await GetUser(userId).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task NonLoggedInUser_IsAuthenticationError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
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

        [Test]
        public async Task NonLoggedInUser_DoesNotDeletePersonalUserData()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            var userId =
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
            await LoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            var response = await GetUser(userId).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Test]
        public async Task MissingPassword_IsUserError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await DeletePersonalUserData(
                null
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Test]
        public async Task MissingPassword_DoesNotDeletePersonalUserData()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            var userId =
                await RegisterAndConfirmAndLoginUser(
                    email: email,
                    password: password
                ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                null
                ).ConfigureAwait(false);
            var response = await GetUser(userId).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Test]
        public async Task IncorrectPassword_IsUserError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await DeletePersonalUserData(
                "incorrect" + password
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Test]
        public async Task IncorrectPassword_DoesNotDeletePersonalUserData()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            var userId =
                await RegisterAndConfirmAndLoginUser(
                    email: email,
                    password: password
                ).ConfigureAwait(false);
            // Act
            await DeletePersonalUserData(
                "incorrect" + password
                ).ConfigureAwait(false);
            var response = await GetUser(userId).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }
    }
}
