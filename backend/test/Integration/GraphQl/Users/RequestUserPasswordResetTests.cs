using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class RequestUserPasswordResetTests
      : UserIntegrationTests
    {
        [Test]
        public async Task ExistingAndConfirmedEmailAddress_RequestsUserPasswordReset()
        {
            // Arrange
            const string name = "John Doe";
            const string email = "john.doe@ise.fraunhofer.de";
            await RegisterAndConfirmUser(
                name: name,
                email: email
                ).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await RequestUserPasswordReset(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailsShouldContainSingle(
                to: (name, email),
                subject: "Reset password",
                bodyRegEx: @"^Please reset your password by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/reset-password\?resetCode=\w+\.$"
                );
        }

        [Test]
        public async Task UnknownEmailAddress_DoesNothing()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            await RegisterAndConfirmUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await RequestUserPasswordReset(
                "unknown." + email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Test]
        public async Task UnconfirmedEmailAddress_DoesNothing()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await RequestUserPasswordReset(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }
    }
}
