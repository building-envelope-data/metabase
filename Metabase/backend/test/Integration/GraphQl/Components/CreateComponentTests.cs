using System.IO;
using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using System;
using Metabase.GraphQl.Components;
using FluentAssertions;

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
                variables: new CreateComponentInput
                (
                    Name: "Component A",
                    Abbreviation: "C!A",
                    Description: "Best component ever!",
                    Availability: null,
                    Categories: Array.Empty<ValueObjects.ComponentCategory>()
                 )
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
                variables: new CreateComponentInput
                (
                    Name: "Component A",
                    Abbreviation: "C!A",
                    Description: "Best component ever!",
                    Availability: null,
                    Categories: Array.Empty<ValueObjects.ComponentCategory>()
                 )
                ).ConfigureAwait(false);
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task LoggedInUser_IsSuccess()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            var response =
            await CreateComponent(
                new CreateComponentInput
                (
                    Name: "Component A",
                    Abbreviation: "C!A",
                    Description: "Best component ever!",
                    Availability: null,
                    Categories: Array.Empty<ValueObjects.ComponentCategory>()
                 )
            ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task LoggedInUser_CreatesComponent()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            // Act
            // TODO Extract id from `CreateComponent` response and check that that id is the one contained in the `Components` response.
            await CreateComponent(
                new CreateComponentInput
                (
                    Name: "Component A",
                    Abbreviation: "C!A",
                    Description: "Best component ever!",
                    Availability: null,
                    Categories: Array.Empty<ValueObjects.ComponentCategory>()
                 )
            ).ConfigureAwait(false);
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.components.edges[*].node.id").Should().NotBeNullOrWhiteSpace()
                 )
                );
        }
    }
}