using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.DataX;

public sealed class AppliedMethod
{
    public AppliedMethod(
        Guid methodId,
        IReadOnlyList<NamedMethodArgument> arguments,
        IReadOnlyList<NamedMethodSource> sources
    )
    {
        MethodId = methodId;
        Arguments = arguments;
        Sources = sources;
    }

    public Guid MethodId { get; }
    public IReadOnlyList<NamedMethodArgument> Arguments { get; }
    public IReadOnlyList<NamedMethodSource> Sources { get; }

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