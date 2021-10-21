using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class ChangeUserPasswordTests
      : UserIntegrationTests
    {
        [Test]
        public async Task ValidData_ChangesUserPassword()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            var newPassword = "new" + password;
            // Act
            var response = await ChangeUserPassword(
                currentPassword: password,
                newPassword: newPassword
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: newPassword
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task NonLoggedInUser_IsAuthenticationError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            var newPassword = "new" + password;
            // Act
            var response = await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ChangeUserPassword.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["currentPassword"] = password,
                    ["newPassword"] = newPassword,
                    ["newPasswordConfirmation"] = newPassword
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task UnconfirmedUser_IsError()
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
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
            );
        }

        [Test]
        public async Task PasswordConfirmationMismatch_IsUserError()
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
                newPassword: "new" + password,
                newPasswordConfirmation: "other" + password
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task PasswordRequiresDigit_IsUserError()
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
                newPassword: "aabb@$CCDD"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task PasswordRequiresLower_IsUserError()
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
                newPassword: "AABB@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task PasswordRequiresNonAlphanumeric_IsUserError()
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
                newPassword: "aaBBccDDeeFF123"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task PasswordRequiresUpper_IsUserError()
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
                newPassword: "aabb@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Test]
        public async Task PasswordTooShort_IsUserError()
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
                newPassword: "aA@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }
    }
}