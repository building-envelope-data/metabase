using System.Text.Json;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
[GraphQLDescription("A parameter given as a primitive or complex JSON value when this method is applied.")]
public sealed class MethodParameter(
    string name,
    JsonElement type
)
{
    [GraphQLDescription("The parameter name.")]
    public string Name { get; private set; } = name;

    [GraphQLDescription("The JSON schema against which a value given for this parameter must be valid.")]
    public JsonElement Type { get; private set; } = type;
}