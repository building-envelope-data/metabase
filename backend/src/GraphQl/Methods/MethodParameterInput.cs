using System.Text.Json;
using HotChocolate;
using Metabase.Data;

namespace Metabase.GraphQl.Methods;

public sealed record MethodParameterInput(
    [GraphQLDescription("The parameter name.")]
    string Name,
    [GraphQLDescription("The JSON schema against which a value given for this parameter must be valid.")]
    JsonElement Type
)
{
    public MethodParameter ToDomainModel()
    {
        return new(
            Name,
            Type
        );
    }
};