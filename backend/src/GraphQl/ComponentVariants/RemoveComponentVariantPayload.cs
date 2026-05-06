using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentVariants;

public sealed class RemoveComponentVariantPayload
{
    private readonly Guid? _oneComponentId;
    private readonly Guid? _otherComponentId;

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
        : this([error])
    {
    }

    public IReadOnlyCollection<RemoveComponentVariantError>? Errors { get; }

    public async Task<Component?> GetOneComponentAsync(
        IComponentByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_oneComponentId is null)
        {
            return null;
        }
        return await byId.LoadAsync(_oneComponentId ?? Guid.Empty, cancellationToken);
    }

    public async Task<Component?> GetOtherComponentAsync(
        IComponentByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_otherComponentId is null)
        {
            return null;
        }
        return await byId.LoadAsync(_otherComponentId ?? Guid.Empty, cancellationToken);
    }
}