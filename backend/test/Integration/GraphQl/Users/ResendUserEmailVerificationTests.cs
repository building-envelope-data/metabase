using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class ResendUserEmailVerificationTests
      : UserIntegrationTests
    {
        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task LoggedInUser_ResendsUserEmailVerification()
        {
            // Arrange
            const string name = "John Doe";
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = DefaultPassword;
            await RegisterAndConfirmAndLoginUser(
                name: name,
                email: email,
                password: password
                ).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await ResendUserEmailVerification().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.resendUserEmailVerification.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
            EmailsShouldContainSingle(
                to: (name, email),
                subject: "Confirm your email",
                bodyRegEx: @"^Please confirm your email address by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email\?email=john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
                );
        }

        [Test]
        [SuppressMessage("Naming", "CA1707")]
        public async Task NonLoggedInUser_IsAuthenticationError()
        {
            // Arrange
            const string email = "john.doe@ise.fraunhofer.de";
            const string password = DefaultPassword;
            await RegisterAndConfirmUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
            // Act
            var response = await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ResendUserEmailVerification.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
    }
}
