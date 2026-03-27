using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.Tests.Integration.GraphQl.Institutions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl.Components;

[TestFixture]
public sealed class GetComponentTests
    : ComponentIntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task NoComponent_Fails()
    {
        // Act
        var response = await GetComponent(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            new Guid("68ccd42538d8490095051f4d0beb2837")
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task UnknownId_Fails()
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
        await CreateComponent(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            MinimalComponentInput with
            {
                ManufacturerId = institutionId
            }
        );
        await LogoutUser();
        // Act
        // There is some tiny probability that the hard-coded identifier is
        // the one of the component in which case this test fails.
        var response = await GetComponent(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            new Guid("68ccd42538d8490095051f4d0beb2837")
        );
        // Assert
        Snapshot.Match(response);
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task KnownId_Succeeds()
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
        var response = await GetComponent(
            AssertHttpSuccess,
            ReadAsString,
            AssertNothing,
            componentIdsAndUuids[1].Uuid
        );
        // Assert
        Snapshot.Match(
            response,
            matchOptions => matchOptions
                .Assert(fieldOptions =>
                    fieldOptions.Field<string>("data.component.id").Should().Be(componentIdsAndUuids[1].Id)
                )
                .Assert(fieldOptions =>
                    fieldOptions.Field<Guid>("data.component.uuid").Should().Be(componentIdsAndUuids[1].Uuid)
                )
        );
    }
}