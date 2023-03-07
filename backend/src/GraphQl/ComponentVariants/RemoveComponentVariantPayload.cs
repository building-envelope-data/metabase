using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentVariants
{
    public sealed class RemoveComponentVariantPayload
    {
        private readonly Guid _oneComponentId;
        private readonly Guid _otherComponentId;
        public IReadOnlyCollection<RemoveComponentVariantError>? Errors { get; }

        public RemoveComponentVariantPayload(
            Guid oneComponentId,
            Guid otherComponentId
            )
        {
            _oneComponentId = oneComponentId;
            _otherComponentId = otherComponentId;
        }

        public RemoveComponentVariantPayload(
            IReadOnlyCollection<RemoveComponentVariantError> errors
            )
        {
            Errors = errors;
        }

        public RemoveComponentVariantPayload(
            RemoveComponentVariantError error
            )
            : this(new[] { error })
        {
        }

        public Task<Data.Component> GetOneComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_oneComponentId, cancellationToken)!;
        }

        public Task<Data.Component> GetOtherComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_otherComponentId, cancellationToken)!;
        }
    }
}