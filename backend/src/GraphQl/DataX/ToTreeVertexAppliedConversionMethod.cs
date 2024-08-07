using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.DataX;

public sealed class ToTreeVertexAppliedConversionMethod
{
    public ToTreeVertexAppliedConversionMethod(
        Guid methodId,
        IReadOnlyList<NamedMethodArgument> arguments,
        string sourceName
    )
    {
        MethodId = methodId;
        Arguments = arguments;
        SourceName = sourceName;
    }

    public Guid MethodId { get; }
    public IReadOnlyList<NamedMethodArgument> Arguments { get; }
    public string SourceName { get; }

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