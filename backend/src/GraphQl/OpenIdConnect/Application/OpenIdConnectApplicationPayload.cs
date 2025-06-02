using Metabase.Data.OpenIdConnect;
using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public class OpenIdConnectApplicationPayload<TApplicationError> : Payload
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