using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [Collection(nameof(Data.User))]
    public sealed class RequestUserPasswordResetTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ExistingAndConfirmedEmailAddress_RequestsUserPasswordReset()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterAndConfirmUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await RequestUserPasswordReset(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailsShouldContainSingle(
                address: email,
                subject: "Reset password",
                messageRegEx: @"^Please reset your password by clicking the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/reset-password\?resetCode=\w+\.$"
                );
        }

        [Fact]
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

        [Fact]
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