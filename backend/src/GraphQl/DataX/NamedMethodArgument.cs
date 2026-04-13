using System.Text.Json;

namespace Metabase.GraphQl.DataX;

public sealed record NamedMethodArgument(
    string Name,
    JsonElement Value
);