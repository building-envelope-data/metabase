using System.Text.RegularExpressions;
using HotChocolate.Types;

namespace Metabase.GraphQl.Scalars;

public sealed class DoiType(
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
    private const string ValidationPattern = "^(10[.][0-z/.]*)$";

    public DoiType()
        : this(
            nameof(DoiType)[..^"Type".Length],
            """
            Digital Object Identifier (DOI) name, that is, a string compliant with [ISO
            26324:2025](https://www.iso.org/standard/88862.html).
            
            See also
            [What is a DOI?](https://www.doi.org/the-identifier/what-is-a-doi/). In short,
            a DOI name is a digital identifier of an object, any object — physical,
            digital, or abstract. DOIs solve a common problem: keeping track of things.
            Things can be matter, material, content, or activities. Designed to be used by
            humans as well as machines, DOIs identify objects persistently. They allow
            things to be uniquely identified and accessed reliably. You know what you have,
            where it is, and others can track it too.
            """
        )
    {
    }
}