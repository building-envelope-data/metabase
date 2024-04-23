using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed record UuidPropositionInput(
        Guid? EqualTo
    );
}