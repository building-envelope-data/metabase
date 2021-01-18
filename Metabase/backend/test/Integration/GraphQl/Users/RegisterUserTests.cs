using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class RegisterUserTests
      : UserIntegrationTests
    {
        [Fact]
        public async Task ValidData_RegistersUser()
        {
            // Act
            var email = "john.doe@ise.fraunhofer.de";
            var response = await RegisterUser(
                email: email,
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                // matchOptions => matchOptions.IgnoreField("data.registerUser.user.id")
                matchOptions => matchOptions.Assert(fieldOptions =>
                  fieldOptions.Field<string>("data.registerUser.user.id").Should().NotBeNullOrWhiteSpace()
                  )
                );
            EmailsShouldContainSingle(
                address: email,
                subject: "Confirm your email",
                messageRegEx: @"^Please confirm your email address with the confirmation code \w+\.$"
                );
        }

        [Fact]
        public async Task PasswordConfirmationMismatch_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aaaAAA123$!@",
                passwordConfirmation: "baaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task DuplicateEmail_IsUserError()
        {
            // Arrange
            await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task InvalidEmail_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doeise.fraunhofer.de",
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresDigit_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aabb@$CCDD"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresLower_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "AABB@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresNonAlphanumeric_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aaBBccDDeeFF123"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresUpper_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aabb@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordTooShort_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "john.doe@ise.fraunhofer.de",
                password: "aA@$567"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task NullOrEmptyEmail_IsUserError()
        {
            // Act
            var response = await RegisterUser(
                email: "",
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }
    }
}
