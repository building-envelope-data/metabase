using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class RemoveComponentAssemblyPayload
    {
        private readonly Data.ComponentAssembly? _association;
        public IReadOnlyCollection<RemoveComponentAssemblyError>? Errors { get; }

        public RemoveComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            _association = componentAssembly;
        }

        public RemoveComponentAssemblyPayload(
            IReadOnlyCollection<RemoveComponentAssemblyError> errors
            )
        {
            Errors = errors;
        }

        public RemoveComponentAssemblyPayload(
            RemoveComponentAssemblyError error
            )
            : this(new[] { error })
        {
        }

        public async Task<Data.Component?> GetAssembledComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            if (_association is null)
            {
                return null;
            }
            return await byId.LoadAsync(_association.AssembledComponentId, cancellationToken)!;
        }

        public async Task<Data.Component?> GetPartComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            if (_association is null)
            {
                return null;
            }
            return await byId.LoadAsync(_association.PartComponentId, cancellationToken)!;
        }
    }
}