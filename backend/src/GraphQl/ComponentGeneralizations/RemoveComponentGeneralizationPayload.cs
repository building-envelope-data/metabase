using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentGeneralizations;

public sealed class RemoveComponentGeneralizationPayload
{
    private readonly Data.ComponentConcretizationAndGeneralization? _association;
    public IReadOnlyCollection<RemoveComponentGeneralizationError>? Errors { get; }

    public RemoveComponentGeneralizationPayload(
        Data.ComponentConcretizationAndGeneralization association
    )
    {
        _association = association;
    }

    public RemoveComponentGeneralizationPayload(
        IReadOnlyCollection<RemoveComponentGeneralizationError> errors
    )
    {
        Errors = errors;
    }

    public RemoveComponentGeneralizationPayload(
        RemoveComponentGeneralizationError error
    )
        : this(new[] { error })
    {
    }

    public async Task<Data.Component?> GetGeneralComponentAsync(
        ComponentByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null) return null;

        return await byId.LoadAsync(_association.GeneralComponentId, cancellationToken)!;
    }

    public async Task<Data.Component?> GetConcreteComponentAsync(
        ComponentByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null) return null;

        return await byId.LoadAsync(_association.ConcreteComponentId, cancellationToken)!;
    }
}