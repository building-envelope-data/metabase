using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metabase.GraphQl.Methods;
using System.Threading;

namespace Metabase.GraphQl.DataX
{
    public sealed class AppliedMethod
    {
        public Guid MethodId { get; }
        // public IReadOnlyList<NamedMethodArgument> Arguments { get; }
        // public IReadOnlyList<NamedMethodSource> Sources { get; }

        public AppliedMethod(
          Guid methodId
        )
        {
            MethodId = methodId;
        }

        public Task<Metabase.Data.Method?> GetMethodAsync(
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
}