using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Metabase.Enumerations;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Components;
using NodaTime;

namespace Metabase.Tests.Integration.GraphQl.Components;

public abstract class ComponentIntegrationTests
    : IntegrationTests
{
    internal static CreateComponentInput MinimalComponentInput { get; } = new(
        null,
        "Component A",
        "C!A",
        "Best component ever!",
        null,
        [],
        null,
        null,
        null,
        null,
        null,
        Guid.Empty
    );

    internal static CreateComponentInput FromAndToRestrictedAvailabilityComponentInput { get; } = new(
        null,
        "Component B",
        "C!B",
        "Another component!",
        new OpenEndedDateTimeRangeInput(
            new OffsetDateTime(new LocalDateTime(2020, 1, 1, 8, 0, 0), Offset.Zero),
            new OffsetDateTime(new LocalDateTime(2020, 11, 5, 20, 0, 0), Offset.Zero)
        ),
        [
            ComponentCategory.MATERIAL,
            ComponentCategory.UNIT
        ],
        null,
        null,
        null,
        null,
        null,
        Guid.Empty
    );

    internal static CreateComponentInput ToRestrictedAvailabilityComponentInput { get; } = new(
        null,
        "Component C",
        "C!C",
        "Yet another component!",
        new OpenEndedDateTimeRangeInput(
            null,
            new OffsetDateTime(new LocalDateTime(2021, 11, 5, 20, 0, 0), Offset.Zero)
        ),
        [
            ComponentCategory.UNIT
        ],
        null,
        null,
        null,
        null,
        null,
        Guid.Empty
    );

    internal static CreateComponentInput FromRestrictedAvailabilityComponentInput { get; } = new(
        null,
        "Component D",
        "C!D",
        "Whatever component!",
        new OpenEndedDateTimeRangeInput(
            new OffsetDateTime(new LocalDateTime(2019, 4, 3, 0, 0, 0), Offset.Zero),
            null
        ),
        [
            ComponentCategory.LAYER,
            ComponentCategory.MATERIAL,
            ComponentCategory.UNIT
        ],
        null,
        null,
        null,
        null,
        null,
        Guid.Empty
    );

    internal static CreateComponentInput CustomIdComponentInput { get; } = new(
        Guid.NewGuid(),
        "Component E",
        "C!E",
        "Custom ID component.",
        null,
        [],
        null,
        null,
        null,
        null,
        null,
        Guid.Empty
    );

    internal static IEnumerable<CreateComponentInput> ComponentInputs
    {
        get
        {
            yield return MinimalComponentInput;
            yield return FromAndToRestrictedAvailabilityComponentInput;
            yield return ToRestrictedAvailabilityComponentInput;
            yield return FromRestrictedAvailabilityComponentInput;
            yield return CustomIdComponentInput;
        }
    }

    internal static IEnumerable<object[]> EnumerateComponentInputs()
    {
        yield return new object[] { nameof(MinimalComponentInput), MinimalComponentInput };
        yield return new object[]
        {
            nameof(FromAndToRestrictedAvailabilityComponentInput), FromAndToRestrictedAvailabilityComponentInput
        };
        yield return new object[]
            { nameof(ToRestrictedAvailabilityComponentInput), ToRestrictedAvailabilityComponentInput };
        yield return new object[]
            { nameof(FromRestrictedAvailabilityComponentInput), FromRestrictedAvailabilityComponentInput };
        yield return new object[]
            { nameof(CustomIdComponentInput), CustomIdComponentInput };
    }

    protected Task<string> GetComponents()
    {
        return GetComponents(HttpClient);
    }

    internal static Task<string> GetComponents(
        HttpClient httpClient
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Components/GetComponents.graphql")
        );
    }

    protected Task<string> GetComponent(
        Guid id
    )
    {
        return GetComponent(HttpClient, id);
    }

    internal static Task<string> GetComponent(
        HttpClient httpClient,
        Guid id
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Components/GetComponent.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["id"] = id
            }
        );
    }

    protected Task<string> CreateComponent(
        CreateComponentInput input
    )
    {
        return CreateComponent(HttpClient, input);
    }

    internal static Task<string> CreateComponent(
        HttpClient httpClient,
        CreateComponentInput input
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
            variables: new { input }
        );
    }

    protected Task<JsonElement> CreateComponentAsJson(
        CreateComponentInput input
    )
    {
        return SuccessfullyQueryGraphQlContentAsJson(
            HttpClient,
            File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
            variables: new { input }
        );
    }

    protected async Task<(string Id, Guid Uuid)> CreateComponentReturningIdAndUuid(
        CreateComponentInput input
    )
    {
        var response = await CreateComponentAsJson(input);
        return (
            ExtractString(
                "$.data.createComponent.component.id",
                response
            ),
            ExtractUuid(
                "$.data.createComponent.component.uuid",
                response
            )
        );
    }
}