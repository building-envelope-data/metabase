using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.NTests.Integration.GraphQl.Institutions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.NTests.Integration.GraphQl.Components
{
    [TestFixture]
    public sealed class GetComponentNTests
      : ComponentIntegrationNTests
    {
        [Test]
        public async Task NoComponent_Fails()
        {
            // Act
            var response = await GetComponent(
                "68ccd42538d8490095051f4d0beb2837"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task UnknownId_Fails()
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
            await CreateComponentReturningIdAndUuid(
                MinimalComponentInput with
                {
                    ManufacturerId = institutionId
                }
                ).ConfigureAwait(false);
            LogoutUser();
            // Act
            // There is some tiny probability that the hard-coded identifier is
            // the one of the component in which case this test fails.
            var response = await GetComponent(
                "68ccd42538d8490095051f4d0beb2837"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task KnownId_Succeeds()
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
            var response = await GetComponent(componentIdsAndUuids[1].Item2).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.component.id").Should().Be(componentIdsAndUuids[1].Item1)
                 )
                .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.component.uuid").Should().Be(componentIdsAndUuids[1].Item2)
                 )
            );
        }
    }
}