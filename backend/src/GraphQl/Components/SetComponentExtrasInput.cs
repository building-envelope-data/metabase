using System;
using System.Text.Json;

namespace Metabase.GraphQl.Components;

public sealed record SetComponentExtrasInput(
    Guid ComponentId,
    JsonElement? Extras
);