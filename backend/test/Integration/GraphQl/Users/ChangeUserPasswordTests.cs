using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Users;

[TestFixture]
public sealed class ChangeUserPasswordTests
    : UserIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task ValidData_ChangesUserPassword()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        const string newPassword = "new" + password;
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            newPassword
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser(
            email,
            newPassword
        );
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
        const string newPassword = "new" + password;
        // Act
        var response = await QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ChangeUserPassword.graphql"),
            new Dictionary<string, object?>
            {
                ["currentPassword"] = password,
                ["newPassword"] = newPassword,
                ["newPasswordConfirmation"] = newPassword
            },
            HttpSuccess,
            AsString,
            ForSnapshotMatch
        );
        // Assert
        Snapshot.Match(response);
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnconfirmedUser_IsError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "new" + password
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
                )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordConfirmationMismatch_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "new" + password,
            "other" + password
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresDigit_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "aabb@$CCDD"
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresLower_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "AABB@$567"
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresNonAlphanumeric_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "aaBBccDDeeFF123"
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordRequiresUpper_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "aabb@$567"
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task PasswordTooShort_IsUserError()
    {
        // Arrange
        const string email = "john.doe@ise.fraunhofer.de";
        const string password = "aaaAAA123$!@";
        await RegisterAndConfirmAndLoginUser(
            email: email,
            password: password
        );
        // Act
        var response = await ChangeUserPassword(
            HttpSuccess,
            AsString,
            ForSnapshotMatch,
            password,
            "aA@$567"
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions.Assert(fieldOptions =>
                fieldOptions.Field<string>("data.changeUserPassword.user.id").Should().NotBeNullOrWhiteSpace()
            )
        );
        await LoginUser();
    }
}