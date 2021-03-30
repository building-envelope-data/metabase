using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.GraphQl.Institutions;
using Snapshooter;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl.Institutions
{
    [Collection(nameof(Data.Institution))]
    public sealed class CreateInstitutionTests
      : InstitutionIntegrationTests
    {
        [Fact]
        public async Task AnonymousUser_IsAuthenticationError()
        {
            // Act
            var response =
                await UnsuccessfullyQueryGraphQlContentAsString(
                    File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
                    variables: OperativeInstitutionInput
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task AnonymousUser_CannotCreateInstitution()
        {
            // Act
            await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
                variables: OperativeInstitutionInput
                ).ConfigureAwait(false);
            var response = await GetInstitutions().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Theory]
        [MemberData(nameof(EnumerateInstitutionInputs))]
        public async Task LoggedInUser_IsSuccess(
            string key,
            CreateInstitutionInput input
        )
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var response = await CreateInstitution(
                input with {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.createInstitution.institution.id").Should().NotBeNullOrWhiteSpace()
                 )
                 .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.createInstitution.institution.uuid").Should().NotBe(Guid.Empty)
                 )
                );
        }

        [Theory]
        [MemberData(nameof(EnumerateInstitutionInputs))]
        public async Task LoggedInUser_CreatesInstitution(
            string key,
            CreateInstitutionInput input
        )
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var (institutionId, institutionUuid) = await CreateInstitutionReturningIdAndUuid(
                input with {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            var response = await GetInstitutions().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
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