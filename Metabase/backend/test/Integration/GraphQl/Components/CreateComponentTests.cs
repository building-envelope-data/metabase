using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using Metabase.GraphQl.Components;
using FluentAssertions;
using Snapshooter;

namespace Metabase.Tests.Integration.GraphQl.Components
{
    [Collection(nameof(Data.Component))]
    public sealed class CreateComponentTests
      : ComponentIntegrationTests
    {
        [Fact]
        public async Task AnonymousUser_IsAuthenticationError()
        {
            // Act
            var response =
             await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                variables: MinimalComponentInput
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task AnonymousUser_CannotCreateComponent()
        {
            // Act
            await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                variables: MinimalComponentInput
                ).ConfigureAwait(false);
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Theory]
        [MemberData(nameof(EnumerateComponentInputs))]
        public async Task LoggedInUser_IsSuccess(
            string key,
            CreateComponentInput input
        )
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var response = await CreateComponent(input).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.createComponent.component.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }

        [Theory]
        [MemberData(nameof(EnumerateComponentInputs))]
        public async Task LoggedInUser_CreatesComponent(
            string key,
            CreateComponentInput input
        )
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var componentId =
                ExtractString(
                    "$.data.createComponent.component.id",
                    await CreateComponentAsJson(input).ConfigureAwait(false)
                );
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.components.edges[*].node.id").Should().Be(componentId)
                 )
                );
        }
    }
}