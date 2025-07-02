using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class SetComponentExtrasPayload
    : ComponentPayload<SetComponentExtrasError>
{
    public SetComponentExtrasPayload(
        Component component
    )
        : base(component)
    {
    }

    public SetComponentExtrasPayload(
        SetComponentExtrasError error
    )
        : base(error)
    {
    }

    public SetComponentExtrasPayload(
        IReadOnlyCollection<SetComponentExtrasError> errors
    )
        : base(errors)
    {
    }
}