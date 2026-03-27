using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

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
        await RegisterAndConfirmAndLoginUser();
        EmailSender.Clear();
        // Act
        var response = await ResendUserEmailVerification(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.resendUserEmailVerification.user.id").Should()
                    .NotBeNullOrWhiteSpace()
            )
        );
        EmailsShouldContainSingle(
            (name, email),
            "Confirm your email",
            $@"^{Regex.Escape($"Please confirm your email address by following the link {AppSettings.Uri.AbsoluteUri}users/confirm-email?email=john.doe@ise.fraunhofer.de&confirmationCode=")}\w+$"
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
        );
        // Act
        var response = await ResendUserEmailVerification(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
        // Assert
        Snapshot.Match(response);
    }
}