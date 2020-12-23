using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public sealed class RegisterUserTests
      : IntegrationTests
    {
        [Fact]
        public async Task PasswordConfirmationMismatch_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordConfirmationMismatch_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ValidData_RegistersUserWithoutErrors()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/ValidData_RegistersUserWithoutErrors.graphql")
              );
          // Assert
          Snapshot.Match(
              await response.Content.ReadAsStringAsync(),
              matchOptions => matchOptions.IgnoreField("data.registerUser.user.id")
              /* matchOptions => matchOptions.Assert( */
              /*   fieldOptions => Assert.NotEqual(Guid.Empty, fieldOptions.Field<Guid>("data.registerUser.user.id")) */
              /*   ) */
              );
        }

        [Fact]
        public async Task DuplicateEmail_IsUserError()
        {
          // Arrange
          await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/DuplicateEmail_IsUserError.graphql")
              );
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/DuplicateEmail_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task InvalidEmail_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/InvalidEmail_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task PasswordRequiresDigit_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresDigit_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task PasswordRequiresLower_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresLower_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task PasswordRequiresNonAlphanumeric_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresNonAlphanumeric_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task PasswordRequiresUpper_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordRequiresUpper_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task PasswordTooShort_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/PasswordTooShort_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task NullOrEmptyEmail_IsUserError()
        {
          // Act
          var response = await QueryGraphQl(
              File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/NullOrEmptyEmail_IsUserError.graphql")
              );
          // Assert
          Snapshot.Match(await response.Content.ReadAsStringAsync());
        }
    }
}
