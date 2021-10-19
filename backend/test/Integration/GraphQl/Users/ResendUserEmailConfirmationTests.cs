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
            var name = "John Doe";
            var email = "john.doe@ise.fraunhofer.de";
            await RegisterUser(
                name: name,
                email: email
                ).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await ResendUserEmailConfirmation(
                email
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailsShouldContainSingle(
                to: (name, email),
                subject: "Confirm your email",
                bodyRegEx: @"^Please confirm your email address by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email\?email=john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
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