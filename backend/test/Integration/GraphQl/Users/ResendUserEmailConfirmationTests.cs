using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ResendUserEmailConfirmationTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ExistingEmailAddress_ResendsUserEmailConfirmation()
    {
        // Arrange
        const string name = "John Doe";
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(
            HttpSuccess,
            AsString,
            ForSnapshotMatch
  );
        EmailSender.Clear();
        // Act
        var response = await ResendUserEmailConfirmation(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            email
        );
        // Assert
        Snapshot.Match(response);
        EmailsShouldContainSingle(
            (name, email),
            "Confirm your email",
            @"^Please confirm your email address by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email\?email=john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownEmailAddress_DoesNothing()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(
            HttpSuccess,
            AsJson,
            NoGraphQlErrors,
            email: email
        );
        EmailSender.Clear();
        // Act
        var response = await ResendUserEmailConfirmation(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            "unknown." + email
        );
        // Assert
        Snapshot.Match(response);
        EmailSender.Emails.Should().BeEmpty();
    }
}