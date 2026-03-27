using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
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
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
  );
        EmailSender.Clear();
        // Act
        var response = await ResendUserEmailConfirmation(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            email
        );
        // Assert
        Snapshot.Match(response);
        EmailsShouldContainSingle(
            (name, email),
            "Confirm your email",
            $@"^{Regex.Escape($"Please confirm your email address by following the link {AppSettings.Uri.AbsoluteUri}users/confirm-email?email=john.doe@ise.fraunhofer.de&confirmationCode=")}\w+$"
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownEmailAddress_DoesNothing()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        await RegisterUser(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            email: email
        );
        EmailSender.Clear();
        // Act
        var response = await ResendUserEmailConfirmation(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            "unknown." + email
        );
        // Assert
        Snapshot.Match(response);
        EmailSender.Emails.Should().BeEmpty();
    }
}