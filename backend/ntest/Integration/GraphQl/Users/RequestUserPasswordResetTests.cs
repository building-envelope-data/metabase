using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.NTests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class RequestUserPasswordResetNTests
      : UserIntegrationNTests
    {
        [Test]
        public async Task ExistingAndConfirmedEmailAddress_RequestsUserPasswordReset()
        {
            // Arrange
            var name = "John Doe";
            var email = "john.doe@ise.fraunhofer.de";
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
                bodyRegEx: @"^Please reset your password by clicking the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/reset-password\?resetCode=\w+\.$"
                );
        }

        [Test]
        public async Task UnknownEmailAddress_DoesNothing()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
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
            var email = "john.doe@ise.fraunhofer.de";
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