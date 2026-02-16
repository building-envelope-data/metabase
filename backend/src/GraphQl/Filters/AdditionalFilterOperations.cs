namespace Metabase.GraphQl.Filters;

public static class AdditionalFilterOperations
{
    // Identifiers must be greater than 1024 according to
    // https://chillicream.com/docs/hotchocolate/v15/api-reference/extending-filtering
    public const int Not = 1025;
    public const int InClosedInterval = 1026;
}