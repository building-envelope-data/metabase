using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class UpdateComponentPayload
    : ComponentPayload<UpdateComponentError>
{
    public UpdateComponentPayload(
        Component component
    )
        : base(component)
    {
    }

    public UpdateComponentPayload(
        UpdateComponentError error
    )
        : base(error)
    {
    }

    public UpdateComponentPayload(
        IReadOnlyCollection<UpdateComponentError> errors
    )
        : base(errors)
    {
    }
}