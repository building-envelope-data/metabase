using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    [TestFixture]
    public sealed class LoginUserTests
      : UserIntegrationTests
    {
        [Test]
        public async Task ValidDataOfRegisteredAndConfirmedUser_LogsInUser()
        {
            // Arrange
            var email = "john.doe@ise.fraunhofer.de";
            var password = "aaaAAA123$!@";
            await RegisterAndConfirmUser(email: email, password: password).ConfigureAwait(false);
            // Act
            var response = await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/LoginUser.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["email"] = email,
                    ["password"] = password,
                    ["rememberMe"] = false
                }
                ).ConfigureAwait(false);
            // Assert
            // TODO assert that cookie was set!
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.loginUser.user.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        // TODO Other cases!
    }
}