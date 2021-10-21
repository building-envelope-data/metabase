using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.GraphQl.Institutions;
using Snapshooter;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl.Institutions
{
    [TestFixture]
    public sealed class CreateInstitutionTests
      : InstitutionIntegrationTests
    {
        [Test]
        public async Task AnonymousUser_IsAuthenticationError()
        {
            // Act
            var response =
                await UnsuccessfullyQueryGraphQlContentAsString(
                    File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
                    variables: PendingInstitutionInput
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task AnonymousUser_CannotCreateInstitution()
        {
            // Act
            await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
                variables: PendingInstitutionInput
                ).ConfigureAwait(false);
            var response = await GetInstitutions().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [TestCaseSource(nameof(EnumerateInstitutionInputs))]
        [Theory]
        public async Task LoggedInUser_IsSuccess(
            string key,
            CreateInstitutionInput input
        )
        {
            SnapshotFullName testName = new SnapshotFullName(SnapshooterNameHelper(nameof(CreateInstitutionTests), nameof(LoggedInUser_IsSuccess),key), SnapshooterDirectoryHelper(nameof(CreateInstitutionTests)));

            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var response = await CreateInstitution(
                input with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                testName,
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.createInstitution.institution.id").Should().NotBeNullOrWhiteSpace()
                 )
                 .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.createInstitution.institution.uuid").Should().NotBe(Guid.Empty)
                 )
                );
        }

        
        [TestCaseSource(nameof(EnumerateInstitutionInputs))]
        [Theory]
        public async Task LoggedInUser_CreatesInstitution(
            string key,
            CreateInstitutionInput input
        )
        {
            SnapshotFullName testName = new SnapshotFullName(SnapshooterNameHelper(nameof(CreateInstitutionTests), nameof(LoggedInUser_CreatesInstitution), key), SnapshooterDirectoryHelper(nameof(CreateInstitutionTests)));

            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var (institutionId, institutionUuid) = await CreateInstitutionReturningIdAndUuid(
                input with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            var response = await GetInstitutions().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                testName,
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.institutions.edges[*].node.id").Should().Be(institutionId)
                 )
                .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.institutions.edges[*].node.uuid").Should().Be(institutionUuid)
                 )
                );
        }
    }
}