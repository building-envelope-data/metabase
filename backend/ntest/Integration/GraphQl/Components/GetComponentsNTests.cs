using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.NTests.Integration.GraphQl.Institutions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.NTests.Integration.GraphQl.Components
{
    [TestFixture]
    //[Collection(nameof(Data.Component))]
    public sealed class GetComponentsNTests
      : ComponentIntegrationNTests
    {
        [Test]
        public async Task NoComponent_ReturnsEmptyList()
        {
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task SingleComponent_IsReturned()
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var institutionId = await InstitutionIntegrationNTests.CreateAndVerifyInstitutionReturningUuid(
                HttpClient,
                InstitutionIntegrationNTests.PendingInstitutionInput with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            var (componentId, componentUuid) = await CreateComponentReturningIdAndUuid(
                MinimalComponentInput with
                {
                    ManufacturerId = institutionId
                }
                ).ConfigureAwait(false);
            LogoutUser();
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.components.edges[*].node.id").Should().Be(componentId)
                 )
                .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.components.edges[*].node.uuid").Should().Be(componentUuid)
                 )
                );
        }

        [Test]
        public async Task MultipleComponents_AreReturned()
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var institutionId = await InstitutionIntegrationNTests.CreateAndVerifyInstitutionReturningUuid(
                HttpClient,
                InstitutionIntegrationNTests.PendingInstitutionInput with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            var componentIdsAndUuids = new List<(string, string)>();
            foreach (var input in ComponentInputs)
            {
                componentIdsAndUuids.Add(
                    await CreateComponentReturningIdAndUuid(
                        input with
                        {
                            ManufacturerId = institutionId
                        }
                        ).ConfigureAwait(false)
                    );
            }
            LogoutUser();
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions =>
                    componentIdsAndUuids.Select(
                        ((string componentId, string componentUuid) componentIdAndUuid, int index)
                         => (componentIdAndUuid.componentId, componentIdAndUuid.componentUuid, index)
                     ).Aggregate(
                        matchOptions,
                        (accumulatedMatchOptions, componentIdAndUuidAndIndex) =>
                            accumulatedMatchOptions
                            .Assert(fieldOptions =>
                                fieldOptions.Field<string>($"data.components.edges[{componentIdAndUuidAndIndex.index}].node.id").Should().Be(componentIdAndUuidAndIndex.componentId)
                                )
                            .Assert(fieldOptions =>
                                fieldOptions.Field<Guid>($"data.components.edges[{componentIdAndUuidAndIndex.index}].node.uuid").Should().Be(componentIdAndUuidAndIndex.componentUuid)
                                )
                )
            );
        }
    }
}