using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Components;

namespace Metabase.Tests.Integration.GraphQl.Components
{
    public abstract class ComponentIntegrationTests
      : IntegrationTests
    {
        protected static CreateComponentInput MinimalComponentInput { get; } = new(
                    Name: "Component A",
                    Abbreviation: "C!A",
                    Description: "Best component ever!",
                    Availability: null,
                    Categories: Array.Empty<ValueObjects.ComponentCategory>()
                 );

        protected static CreateComponentInput FromAndToRestrictedAvailabilityComponentInput { get; } = new(
                    Name: "Component B",
                    Abbreviation: "C!B",
                    Description: "Another component!",
                    Availability: new OpenEndedDateTimeRangeInput(
                      From: new DateTime(2020, 1, 1, 8, 0, 0, DateTimeKind.Utc),
                      To: new DateTime(2020, 11, 5, 20, 0, 0, DateTimeKind.Utc)
                      ),
                    Categories: new[] {
                        ValueObjects.ComponentCategory.MATERIAL,
                        ValueObjects.ComponentCategory.UNIT,
                        }
                 );

        protected static CreateComponentInput ToRestrictedAvailabilityComponentInput { get; } = new(
                    Name: "Component C",
                    Abbreviation: "C!C",
                    Description: "Yet another component!",
                    Availability: new OpenEndedDateTimeRangeInput(
                      From: null,
                      To: new DateTime(2021, 11, 5, 20, 0, 0, DateTimeKind.Utc)
                      ),
                    Categories: new[] {
                        ValueObjects.ComponentCategory.UNIT,
                        }
                 );

        protected static CreateComponentInput FromRestrictedAvailabilityComponentInput { get; } = new(
                    Name: "Component D",
                    Abbreviation: "C!D",
                    Description: "Whatever component!",
                    Availability: new OpenEndedDateTimeRangeInput(
                      From: new DateTime(2019, 4, 3, 0, 0, 0, DateTimeKind.Utc),
                      To: null
                      ),
                    Categories: new[] {
                        ValueObjects.ComponentCategory.LAYER,
                        ValueObjects.ComponentCategory.MATERIAL,
                        ValueObjects.ComponentCategory.UNIT,
                        }
                 );

        protected static IEnumerable<object[]> EnumerateComponentInputs()
        {
            yield return new object[] { nameof(MinimalComponentInput), MinimalComponentInput };
            yield return new object[] { nameof(FromAndToRestrictedAvailabilityComponentInput), FromAndToRestrictedAvailabilityComponentInput };
            yield return new object[] { nameof(ToRestrictedAvailabilityComponentInput), ToRestrictedAvailabilityComponentInput };
            yield return new object[] { nameof(FromRestrictedAvailabilityComponentInput), FromRestrictedAvailabilityComponentInput };
        }

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

        protected Task<JsonElement> CreateComponentAsJson(
            CreateComponentInput input
            )
        {
            return SuccessfullyQueryGraphQlContentAsJson(
                File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                variables: input
                );
        }
    }
}