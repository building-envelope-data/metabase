using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.GraphQl.Institutions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Institutions;

[TestFixture]
public sealed class CreateInstitutionTests
    : InstitutionIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task AnonymousUser_IsAuthenticationError()
    {
        // Act
        var response =
            await QueryGraphQl(
                File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
                new { input = PendingInstitutionInput },
                AssertHttpSuccess,
                ReadAsString,
                AssertNothing
            );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task AnonymousUser_CannotCreateInstitution()
    {
        // Act
        await QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
            new { input = PendingInstitutionInput },
            AssertHttpSuccess,
            ReadAsJson,
            AssertHasGraphQlErrors
        );
        var response = await GetInstitutions(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
        // Assert
        // The existing institution was created by the database seeder run in `Program.cs`
        Snapshot.Match(
            response,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.institutions.edges[0].node.id").Should()
                        .NotBeNullOrWhiteSpace()
                )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.institutions.edges[0].node.uuid").Should().NotBe(Guid.Empty)
                )
        );
    }

    [TestCaseSource(nameof(EnumerateInstitutionInputs))]
    [Theory]
    [SuppressMessage("Naming", "CA1707")]
    public async Task LoggedInUser_IsSuccess(
        string key,
        CreateInstitutionInput input
    )
    {
        var testName = SnapshotFullNameHelper(typeof(CreateInstitutionTests), key);

        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        // Act
        var response = await CreateInstitution(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            input with
            {
                OwnerIds = [userId]
            }
        );
        // Assert
        Snapshot.Match(
            response,
            testName,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.createInstitution.institution.id").Should()
                        .NotBeNullOrWhiteSpace()
                )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.createInstitution.institution.uuid").Should().NotBe(Guid.Empty)
                )
        );
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task LoggedInUser_IsSuccessWithCustomId()
    {
        var input = CustomIdInstitutionInput;

        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        // Act
        var response = await CreateInstitution(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            input with
            {
                OwnerIds = [userId]
            }
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.createInstitution.institution.id").Should()
                        .NotBeNullOrWhiteSpace()
                )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.createInstitution.institution.uuid").Should().Be(input.InstitutionId ?? Guid.Empty)
                )
        );
    }

    [TestCaseSource(nameof(EnumerateInstitutionInputs))]
    [Theory]
    [SuppressMessage("Naming", "CA1707")]
    public async Task LoggedInUser_CreatesInstitution(
        string key,
        CreateInstitutionInput input
    )
    {
        var testName = SnapshotFullNameHelper(typeof(CreateInstitutionTests), key);

        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        // Act
        var (institutionId, institutionUuid) = await CreateInstitutionReturningIdAndUuid(
            input with
            {
                OwnerIds = [userId]
            }
        );
        var response = await GetPendingInstitutions(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
        // Assert
        Snapshot.Match(
            response,
            testName,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.pendingInstitutions.edges[*].node.id").Should().Be(institutionId)
                )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.pendingInstitutions.edges[*].node.uuid").Should().Be(institutionUuid)
                )
        );
    }
}