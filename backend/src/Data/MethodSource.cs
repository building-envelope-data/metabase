using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
[GraphQLDescription("A data source given as a cross-database data reference when this method is applied.")]
public sealed class MethodSource(
    string name,
    string description
)
{
    [GraphQLDescription("The source name.")]
    public string Name { get; private set; } = name;

    [GraphQLDescription("The significance of the source for the method.")]
    public string Description { get; private set; } = description;
}