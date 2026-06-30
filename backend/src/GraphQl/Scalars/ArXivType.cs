using System.Text.RegularExpressions;
using HotChocolate.Types;

namespace Metabase.GraphQl.Scalars;

public sealed class ArXivType(
    string name,
    string? description = null,
    BindingBehavior bind = BindingBehavior.Explicit
)
: RegexType(
    name,
    ValidationPattern,
    description,
    RegexOptions.Compiled | RegexOptions.IgnoreCase,
    bind
)
{
    private const string ValidationPattern = "^(arXiv:)[0-z./]*$";

    public ArXivType()
        : this(
            nameof(ArXivType)[..^"Type".Length],
            """
            arXiv identifier, that is, a string of the format
            [Understanding the arXiv identifier](https://info.arxiv.org/help/arxiv_identifier.html).
            """
        )
    {
    }
}