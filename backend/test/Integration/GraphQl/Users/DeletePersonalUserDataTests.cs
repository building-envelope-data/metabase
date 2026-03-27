using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class DeletePersonalUserDataTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task CorrectPassword_IsSuccess()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            password
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task CorrectPassword_DeletesPersonalUserData()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var userId =
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            );
        // Act
        await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            password
        );
        var response = await GetUser(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            userId
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NonLoggedInUser_IsAuthenticationError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmUser(
            email: email,
            password: password
        );
        // Act
        var response = await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            password
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NonLoggedInUser_DoesNotDeletePersonalUserData()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var userId =
            await RegisterAndConfirmUser(
                email: email,
                password: password
            );
        // Act
        await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsJson,
            AssertHasGraphQlErrors,
            password
        );
        await LoginUser();
        var response = await GetUser(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            userId
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task MissingPassword_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            null
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task MissingPassword_DoesNotDeletePersonalUserData()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var userId =
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            );
        // Act
        await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsJson,
            AssertHasGraphQlErrors,
            null
        );
        var response = await GetUser(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            userId
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task IncorrectPassword_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            "incorrect" + password
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.deletePersonalUserData.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task IncorrectPassword_DoesNotDeletePersonalUserData()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        var userId =
            await RegisterAndConfirmAndLoginUser(
                email: email,
                password: password
            );
        // Act
        await DeletePersonalUserData(
            AssertHttpSuccess,
            ReadAsJson,
            AssertHasGraphQlErrors,
            "incorrect" + password
        );
        var response = await GetUser(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            userId
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
    }
}