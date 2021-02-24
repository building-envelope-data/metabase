using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [Collection(nameof(Data.User))]
    public sealed class ResendUserEmailConfirmationTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ExistingEmailAddress_ResendsUserEmailConfirmation()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailsShouldContainSingle(
                address: email,
                subject: "Confirm your email",
                messageRegEx: @"^Please confirm your email address with the confirmation code \w+\.$"
                );
        }

        [Fact]
        public async Task UnknownEmailAddress_DoesNothing()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(email: email).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await ResendUserEmailConfirmation(
                "unknown." + email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }
    }
}