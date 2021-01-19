using System.IO;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;

namespace Metabase.Tests.Integration.GraphQl.Components
{
    public abstract class ComponentIntegrationTests
      : IntegrationTests
    {
        protected Task<string> GetComponents()
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Components/Components.graphql")
                );
        }

        protected Task<string> CreateComponent(
            CreateComponentInput input
            )
        {
            return SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                variables: input
                );
        }
    }
}