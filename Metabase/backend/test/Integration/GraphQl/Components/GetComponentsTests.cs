using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;

namespace Metabase.Tests.Integration.GraphQl.Components
{
    [Collection(nameof(Data.Component))]
    public sealed class GetComponentsTests
      : ComponentIntegrationTests
    {
        [Fact]
        public async Task NoComponent_ReturnsEmptyList()
        {
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task SingleComponent_IsReturned()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var componentId = await CreateComponentReturningId(MinimalComponentInput).ConfigureAwait(false);
            LogoutUser();
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.components.edges[*].node.id").Should().Be(componentId)
                 )
                );
        }

        [Fact]
        public async Task MultipleComponents_AreReturned()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var componentIds = new List<string>();
            foreach (var input in ComponentInputs)
            {
                componentIds.Add(
                    await CreateComponentReturningId(input).ConfigureAwait(false)
                    );
            }
            LogoutUser();
            // Act
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions =>
                    componentIds.Select((componentId, index) => (componentId, index)).Aggregate(
                        matchOptions,
                        (accumulatedMatchOptions, componentIdAndIndex) =>
                            accumulatedMatchOptions.Assert(fieldOptions =>
                                fieldOptions.Field<string>($"data.components.edges[{componentIdAndIndex.index}].node.id").Should().Be(componentIdAndIndex.componentId)
                                )
                )
            );
        }
    }
}