using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [Collection(nameof(Data.User))]
    public sealed class ChangeUserEmailTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ValidData_EmailsConfirmationCode()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                name: name,
                email: email,
                password: password
            ).ConfigureAwait(false);
            EmailSender.Clear();
            var newEmail = "new." + email;
            // Act
            var response = await ChangeUserEmail(
                newEmail
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
            EmailsShouldContainSingle(
                to: (name, newEmail),
                subject: "Confirm your email change",
                bodyRegEx: @"^Please confirm your email address change by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email-change\?currentEmail=john\.doe@ise\.fraunhofer\.de&newEmail=new.john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
                );
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
            var newEmail = "new." + email;
            // Act
            var response = await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ChangeUserEmail.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["newEmail"] = newEmail
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Fact]
        public async Task UnchangedEmail_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await ChangeUserEmail(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
            );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }

        [Fact]
        public async Task InvalidEmail_IsUserError()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            var newEmail = "@invalid@" + email;
            // Act
            var response = await ChangeUserEmail(
                newEmail
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.changeUserEmail.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            await LoginUser(
                email: email,
                password: password
                ).ConfigureAwait(false);
        }
    }
}