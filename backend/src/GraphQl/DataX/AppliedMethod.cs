using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.DataX;

public sealed class AppliedMethod
{
    // public IReadOnlyList<NamedMethodArgument> Arguments { get; }
    // public IReadOnlyList<NamedMethodSource> Sources { get; }

    public AppliedMethod(
        Guid methodId
    )
    {
        MethodId = methodId;
    }

    public Guid MethodId { get; }

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