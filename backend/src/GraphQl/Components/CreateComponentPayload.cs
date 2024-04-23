using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class CreateComponentPayload
    : ComponentPayload<CreateComponentError>
{
    public CreateComponentPayload(
        Component component
    )
        : base(component)
    {
    }

    public CreateComponentPayload(
        CreateComponentError error
    )
        : base(error)
    {
    }

    public CreateComponentPayload(
        IReadOnlyCollection<CreateComponentError> errors
    )
        : base(errors)
    {
    }
}