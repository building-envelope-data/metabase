using System.Text.Json;
using GraphQL.Client.Abstractions.Utilities;

namespace Metabase.Json;

/// <summary>
/// Robust conversion to camel case that in particular converts constant case to
/// camel case.
/// </summary>
public sealed class ToCamelCaseJsonNamingPolicy
 : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToCamelCase();
}