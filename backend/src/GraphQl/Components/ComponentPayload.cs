using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public abstract class ComponentPayload<TComponentError>
    : Payload
    where TComponentError : IUserError
{
    protected ComponentPayload(
        Component component
    )
    {
        Component = component;
    }

    protected ComponentPayload(
        IReadOnlyCollection<TComponentError> errors
    )
    {
        Errors = errors;
    }

    protected ComponentPayload(
        TComponentError error
    )
        : this(new[] { error })
    {
    }

    protected ComponentPayload(
        Component component,
        IReadOnlyCollection<TComponentError> errors
    )
    {
        Component = component;
        Errors = errors;
    }

    protected ComponentPayload(
        Component component,
        TComponentError error
    )
        : this(
            component,
            new[] { error }
        )
    {
    }

    public Component? Component { get; }
    public IReadOnlyCollection<TComponentError>? Errors { get; }
}