using System.Threading.Tasks;
using Snapshooter.Xunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

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
                "Q29tcG9uZW50CmdiYzZlNGM5ZDM4Y2M0MWJjODllM2Y2MjkxNmIyYmI1Yg=="
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task UnknownId_Fails()
        {
            // Arrange
            await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            await CreateComponentReturningId(MinimalComponentInput).ConfigureAwait(false);
            LogoutUser();
            // Act
            // There is some tiny probability that the hard-coded identifier is
            // the one of the component in which case this test fails.
            var response = await GetComponent(
                "Q29tcG9uZW50CmdjYjMwMjI4ZTA4Zjk0ZTNiYjY4NTA1ODA1NWYyY2I0Mw=="
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Fact]
        public async Task KnownId_Succeeds()
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
            var response = await GetComponent(componentIds[1]).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                matchOptions => matchOptions.Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.component.id").Should().Be(componentIds[1])
                 )
            );
        }
    }
}