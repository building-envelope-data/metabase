using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.DataX;

public sealed record AppliedMethod(
    Guid MethodId,
    IReadOnlyList<NamedMethodArgument> Arguments,
    IReadOnlyList<NamedMethodSource> Sources
    )
{
    public Task<Method?> GetMethodAsync(
        MethodByIdDataLoader methodById,
        CancellationToken cancellationToken
    )
    {
        return methodById.LoadAsync(
            MethodId,
            cancellationToken
        );
    }
}