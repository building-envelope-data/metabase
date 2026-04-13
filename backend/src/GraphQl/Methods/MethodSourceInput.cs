using HotChocolate;
using Metabase.Data;

namespace Metabase.GraphQl.Methods;

[GraphQLDescription("A data source given as a cross-database data reference when this method is applied.")]
public sealed record MethodSourceInput(
    [GraphQLDescription("The source name.")]
    string Name,
    [GraphQLDescription("The significance of the source for the method.")]
    string Description
)
{
    public MethodSource ToDomainModel()
    {
        return new(
            Name,
            Description
        );
    }
};