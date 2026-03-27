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
        var response = await GetComponents(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task SingleComponent_IsReturned()
    {
        // Arrange
        var userId = await RegisterAndConfirmAndLoginUser();
        var institutionId = await InstitutionIntegrationTests.CreateInstitutionReturningUuid(
            HttpClient,
            InstitutionIntegrationTests.PendingInstitutionInput with
            {
                OwnerIds = [userId]
            }
        );
        await AsVerifier(httpClient =>
            InstitutionIntegrationTests.VerifyInstitution(
                httpClient,
                AssertHttpSuccess,
                ReadAsJson,
                AssertNoGraphQlErrors,
                institutionId
            )
        );
        var (componentId, componentUuid) = await CreateComponentReturningIdAndUuid(
            MinimalComponentInput with
            {
                ManufacturerId = institutionId
            }
        );
        await LogoutUser();
        // Act
        var response = await GetComponents(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing
        );
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
        var institutionId = await InstitutionIntegrationTests.CreateInstitutionReturningUuid(
            HttpClient,
            InstitutionIntegrationTests.PendingInstitutionInput with
            {
                OwnerIds = [userId]
            }
        );
        await AsVerifier(httpClient =>
            InstitutionIntegrationTests.VerifyInstitution(
                httpClient,
                AssertHttpSuccess,
                ReadAsJson,
                AssertNoGraphQlErrors,
                institutionId
            )
        );
        var componentIdsAndUuids = new List<(string Id, Guid Uuid)>();
        foreach (var input in ComponentInputs.OrderBy(_ => _.Name))
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
        var response = await GetComponents(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            new { order = new object[] { new { name = "ASC" } } }
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions =>
                componentIdsAndUuids
                .Index()
                .Aggregate(
                    matchOptions,
                    (accumulatedMatchOptions, componentIdAndUuidAndIndex) =>
                        accumulatedMatchOptions
                            .Assert(fieldOptions =>
                                fieldOptions
                                    .Field<string>(
                                        $"data.components.edges[{componentIdAndUuidAndIndex.Index}].node.id")
                                    .Should().Be(componentIdAndUuidAndIndex.Item.Id)
                            )
                            .Assert(fieldOptions =>
                                fieldOptions
                                    .Field<Guid>(
                                        $"data.components.edges[{componentIdAndUuidAndIndex.Index}].node.uuid")
                                    .Should().Be(componentIdAndUuidAndIndex.Item.Uuid)
                            )
                )
        );
    }
}