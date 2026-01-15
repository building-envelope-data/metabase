using System.Collections.Immutable;

namespace Metabase.GraphQl;

internal static class GraphQlConstants
{
    internal const string EndpointPath = "/graphql";
    internal const string CorsPolicy = "GraphQlCorsPolicy";
    internal const string TypeDiscriminatorPropertyName = "__typename";
    internal const string FilterInputSuffix = "FilterInput";
}