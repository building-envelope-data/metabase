using System.Text.Json;
using GraphQL.Client.Abstractions.Utilities;

namespace Metabase.Json;

/// <summary>
/// Robust conversion to constant case.
/// </summary>
public sealed class ToConstantCaseJsonNamingPolicy
 : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToConstantCase();
}