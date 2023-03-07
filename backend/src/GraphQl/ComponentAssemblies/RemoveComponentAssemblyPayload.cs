using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class RemoveComponentAssemblyPayload
    {
        private readonly Guid _assembledComponentId;
        private readonly Guid _partComponentId;
        public IReadOnlyCollection<RemoveComponentAssemblyError>? Errors { get; }

        public RemoveComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            _assembledComponentId = componentAssembly.AssembledComponentId;
            _partComponentId = componentAssembly.PartComponentId;
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

        public Task<Data.Component> GetAssembledComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_assembledComponentId, cancellationToken)!;
        }

        public Task<Data.Component> GetPartComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_partComponentId, cancellationToken)!;
        }
    }
}