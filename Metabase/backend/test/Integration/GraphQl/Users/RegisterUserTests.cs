using System;
using FluentAssertions;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;
using System.Linq;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class RegisterUserTests
      : IntegrationTests
    {
        [Fact]
        public async Task ValidData_RegistersUserWithoutErrors()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/ValidData_RegistersUserWithoutErrors.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.IgnoreField("data.registerUser.user.id")
                /* matchOptions => matchOptions.Assert( */
                /*   fieldOptions => Assert.NotEqual(Guid.Empty, fieldOptions.Field<Guid>("data.registerUser.user.id")) */
                /*   ) */
                );
            EmailsShouldContainSingle(
                address: "john.doe@ise.fraunhofer.de",
                subject: "Confirm your email",
                messageRegEx: @"^Please confirm your email address with the confirmation code \w+\.$"
            );
        }

        [Fact]
        public async Task PasswordConfirmationMismatch_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordConfirmationMismatch_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task DuplicateEmail_IsUserError()
        {
            // Arrange
            await QueryGraphQl(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/DuplicateEmail_IsUserError.graphql")
                ).ConfigureAwait(false);
            EmailSender.Clear();
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/DuplicateEmail_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task InvalidEmail_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/InvalidEmail_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresDigit_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresDigit_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresLower_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresLower_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresNonAlphanumeric_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresNonAlphanumeric_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordRequiresUpper_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresUpper_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task PasswordTooShort_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordTooShort_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }

        [Fact]
        public async Task NullOrEmptyEmail_IsUserError()
        {
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/NullOrEmptyEmail_IsUserError.graphql")
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
            EmailSender.Emails.Should().BeEmpty();
        }
    }
}
