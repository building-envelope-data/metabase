using System.Collections.Generic;

namespace Metabase.GraphQl.Methods;

public abstract class MethodPayload<TMethodError>
    : Payload
    where TMethodError : IUserError
{
    public Data.Method? Method { get; }
    public IReadOnlyCollection<TMethodError>? Errors { get; }

    protected MethodPayload(
        Data.Method method
    )
    {
        Method = method;
    }

    protected MethodPayload(
        IReadOnlyCollection<TMethodError> errors
    )
    {
        Errors = errors;
    }

    protected MethodPayload(
        TMethodError error
    )
        : this(new[] { error })
    {
    }

    protected MethodPayload(
        Data.Method method,
        IReadOnlyCollection<TMethodError> errors
    )
    {
        Method = method;
        Errors = errors;
    }

    protected MethodPayload(
        Data.Method method,
        TMethodError error
    )
        : this(
            method,
            new[] { error }
        )
    {
    }
}