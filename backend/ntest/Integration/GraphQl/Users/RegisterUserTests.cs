using System;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class RegisterUserTests
      : UserIntegrationTests
    {
        [Test]
        public async Task ValidData_RegistersUser()
        {
            // Act
            var name = "John Doe";
            var email = "john.doe@ise.fraunhofer.de";
            var response = await RegisterUser(
                email: email,
                password: "aaaAAA123$!@"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                // matchOptions => matchOptions.IgnoreField("data.registerUser.user.id")
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.registerUser.user.id").Should().NotBeNullOrWhiteSpace()
                    )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.registerUser.user.uuid").Should().NotBe(Guid.Empty)
                    )
                );
            EmailsShouldContainSingle(
                to: (name, email),
                subject: "Confirm your email",
                bodyRegEx: @"^Please confirm your email address by following the link https:\/\/local\.buildingenvelopedata\.org:4041\/users\/confirm-email\?email=john\.doe@ise\.fraunhofer\.de&confirmationCode=\w+$"
                );
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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