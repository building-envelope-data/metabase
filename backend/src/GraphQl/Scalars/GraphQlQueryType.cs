using HotChocolate.Types;

namespace Metabase.GraphQl.Scalars;

public sealed class GraphQlQueryType(
    string name,
    string? description = null,
    BindingBehavior bind = BindingBehavior.Explicit
)
: StringType(
    name,
    description,
    bind
)
{
    public GraphQlQueryType()
        : this(
            nameof(GraphQlQueryType)[..^"Type".Length],
            """
            [September 2025 GraphQL](https://spec.graphql.org/September2025/)
            compliant
            [query](https://spec.graphql.org/September2025/#sec-Language.Operations)
            string without
            [variables](http://spec.graphql.org/September2025/#sec-Language.Variables)
            and without unnecessary white-space for the present schema.
            """
        )
    {
    }
}