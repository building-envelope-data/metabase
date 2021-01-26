using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace Metabase.Tests.Integration.GraphQl.Components
{
    [Collection(nameof(Data.Component))]
    public sealed class GetComponentTests
      : ComponentIntegrationTests
    {
        [Fact]
        public async Task NoComponent_Fails()
        {
            // Act
            var response = await GetComponent(
                "68ccd42538d8490095051f4d0beb2837"
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task UnknownId_Fails()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            await CreateComponentReturningIdAndUuid(MinimalComponentInput).ConfigureAwait(false);
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

        [Fact]
        public async Task KnownId_Succeeds()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var componentIdsAndUuids = new List<(string, string)>();
            foreach (var input in ComponentInputs)
            {
                componentIdsAndUuids.Add(
                    await CreateComponentReturningIdAndUuid(input).ConfigureAwait(false)
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