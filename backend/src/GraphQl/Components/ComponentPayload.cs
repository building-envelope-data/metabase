using System.Collections.Generic;

namespace Metabase.GraphQl.Components;

public abstract class ComponentPayload<TComponentError>
    : Payload
    where TComponentError : IUserError
{
    public Data.Component? Component { get; }
    public IReadOnlyCollection<TComponentError>? Errors { get; }

    protected ComponentPayload(
        Data.Component component
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
        Data.Component component,
        IReadOnlyCollection<TComponentError> errors
    )
    {
        Component = component;
        Errors = errors;
    }

    protected ComponentPayload(
        Data.Component component,
        TComponentError error
    )
        : this(
            component,
            new[] { error }
        )
    {
    }
}