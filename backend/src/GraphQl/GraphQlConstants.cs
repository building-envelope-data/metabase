namespace Metabase.GraphQl;

internal static class GraphQlConstants
{
    internal const string EndpointPath = "/graphql";
    internal const string CorsPolicy = "GraphQlCorsPolicy";
    internal const string TypeDiscriminatorPropertyName = "__typename";
    internal const string FilterInputSuffix = "PropositionInput";
    internal const string SortInputSuffix = "SortInput";
    internal const string PendingPrefix = "pending";
    internal const string UuidFieldName = "uuid";
    internal const string VersionFieldName = "version";
}