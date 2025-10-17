using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.Tests.Integration.GraphQl.Institutions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Components;

[TestFixture]
public sealed class GetComponentsTests
    : ComponentIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NoComponent_ReturnsEmptyList()
    {
        // Act
        var response = await GetComponents();
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task SingleComponent_IsReturned()
    {
        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        var institutionId = await InstitutionIntegrationTests.CreateAndVerifyInstitutionReturningUuid(
            HttpClient,
            AppSettings.BootstrapUserPassword,
            InstitutionIntegrationTests.PendingInstitutionInput with
            {
                OwnerIds = [userId]
            }
        );
        var (componentId, componentUuid) = await CreateComponentReturningIdAndUuid(
            MinimalComponentInput with
            {
                ManufacturerId = institutionId
            }
        );
        await LogoutUser();
        // Act
        var response = await GetComponents();
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
    [SuppressMessage("Naming", "CA1707")]
    public async Task MultipleComponents_AreReturned()
    {
        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        var institutionId = await InstitutionIntegrationTests.CreateAndVerifyInstitutionReturningUuid(
            HttpClient,
            AppSettings.BootstrapUserPassword,
            InstitutionIntegrationTests.PendingInstitutionInput with
            {
                OwnerIds = [userId]
            }
        );
        var componentIdsAndUuids = new List<(string Id, Guid Uuid)>();
        foreach (var input in ComponentInputs)
        {
            componentIdsAndUuids.Add(
                await CreateComponentReturningIdAndUuid(
                    input with
                    {
                        ManufacturerId = institutionId
                    }
                )
            );
        }

        await LogoutUser();
        // Act
        var response = await GetComponents();
        // Assert
        Snapshot.Match(
            response,
            matchOptions =>
                componentIdsAndUuids.Select(
                    ((string componentId, Guid componentUuid) componentIdAndUuid, int index)
                        => (componentIdAndUuid.componentId, componentIdAndUuid.componentUuid, index)
                ).Aggregate(
                    matchOptions,
                    (accumulatedMatchOptions, componentIdAndUuidAndIndex) =>
                        accumulatedMatchOptions
                            .Assert(fieldOptions =>
                                fieldOptions
                                    .Field<string>(
                                        $"data.components.edges[{componentIdAndUuidAndIndex.index}].node.id")
                                    .Should().Be(componentIdAndUuidAndIndex.componentId)
                            )
                            .Assert(fieldOptions =>
                                fieldOptions
                                    .Field<Guid>(
                                        $"data.components.edges[{componentIdAndUuidAndIndex.index}].node.uuid")
                                    .Should().Be(componentIdAndUuidAndIndex.componentUuid)
                            )
                )
        );
    }
}