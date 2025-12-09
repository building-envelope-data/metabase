using System.Collections.Generic;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public abstract class OpenIdConnectApplicationPayload<TApplicationError> : Payload
    where TApplicationError : IUserError
{
    protected OpenIdConnectApplicationPayload(
        OpenIdConnectApplication application
    )
    {
        Application = application;
    }

    protected OpenIdConnectApplicationPayload(
        IReadOnlyCollection<TApplicationError> errors
    )
    {
        Errors = errors;
    }

    protected OpenIdConnectApplicationPayload(
        TApplicationError error
    )
        : this([error])
    {
    }

    protected OpenIdConnectApplicationPayload(
        OpenIdConnectApplication application,
        IReadOnlyCollection<TApplicationError> errors
    )
    {
        Application = application;
        Errors = errors;
    }

    protected OpenIdConnectApplicationPayload(
        OpenIdConnectApplication application,
        TApplicationError error
    )
        : this(
            application,
            [error]
        )
    {
    }

    public OpenIdConnectApplication? Application { get; }
    public IReadOnlyCollection<TApplicationError>? Errors { get; }
}