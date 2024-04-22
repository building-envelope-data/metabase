using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class ChangeUserEmailTests
      : UserIntegrationTests
    {
        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task ValidData_EmailsConfirmationCode()
        {
            // Arrange
            const string name = "John Doe";
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                name: name,
                email: email,
                password: password
            ).ConfigureAwait(false);
            EmailSender.Clear();
            const string newEmail = "new." + email;
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
                recipient: (name, newEmail),
                subject: "Confirm your email change",
                bodyRegEx: @"^Please confirm your email address change by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email-change\?currentEmail=john\.doe@ise\.fraunhofer\.de&newEmail=new.john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
                );
        }

        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task NonLoggedInUser_IsAuthenticationError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            await RegisterAndConfirmUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            const string newEmail = "new." + email;
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

        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task UnchangedEmail_IsUserError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
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

        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task InvalidEmail_IsUserError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = "aaaAAA123$!@";
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            const string newEmail = "@invalid@" + email;
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
