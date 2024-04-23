using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentGeneralizations;

public sealed class AddComponentGeneralizationPayload
{
    public ComponentGeneralizationOfEdge? GeneralizationOfEdge { get; }
    public ComponentConcretizationOfEdge? ConcretizationOfEdge { get; }
    public IReadOnlyCollection<AddComponentGeneralizationError>? Errors { get; }

    public AddComponentGeneralizationPayload(
        Data.ComponentConcretizationAndGeneralization association
    )
    {
        GeneralizationOfEdge = new ComponentGeneralizationOfEdge(association);
        ConcretizationOfEdge = new ComponentConcretizationOfEdge(association);
    }

    public AddComponentGeneralizationPayload(
        IReadOnlyCollection<AddComponentGeneralizationError> errors
    )
    {
        Errors = errors;
    }

    public AddComponentGeneralizationPayload(
        AddComponentGeneralizationError error
    )
        : this(new[] { error })
    {
    }
}