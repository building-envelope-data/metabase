using System;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Metabase.Authentication;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to authenticate the claims principal in the HTTP context.")]
    internal static partial void FailedToAuthenticate(
        this ILogger<GraphQlAuthenticationAndAntiforgeryHandler> logger,
        Exception exception
    );
}

internal sealed class GraphQlAuthenticationAndAntiforgeryHandler(
    AuthenticationHandler authenticationHandler,
    IAntiforgery antiforgeryService,
    ILogger<GraphQlAuthenticationAndAntiforgeryHandler> logger
)
{
    private async Task AuthenticateAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        httpContext.User = new ClaimsPrincipal(); // clear possibly-existing identities
        try
        {
            var authenticateResult = await authenticationHandler.AuthenticateAsync(httpContext, cancellationToken);
            if (authenticateResult is { Succeeded: true, Principal.Identity.IsAuthenticated: true })
            {
                httpContext.User = authenticateResult.Principal;
            }
        }
        catch (Exception exception)
        {
            logger.FailedToAuthenticate(exception);
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage("Failed to authenticate the claims principal in the HTTP context.")
                .SetException(exception)
                .Build()
            );
        }
    }

    private async Task ValidateAntiforgeryTokenAsync(
        HttpContext httpContext
    )
    {
        try
        {
            await antiforgeryService.ValidateRequestAsync(httpContext);
        }
        catch (AntiforgeryValidationException exception)
        {
            throw new GraphQLException(
                ErrorBuilder
                .New()
                .SetMessage("The antiforgery token is invalid.")
                .SetException(exception)
                .Build()
            );
        }
    }

    public async Task HandleAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        await AuthenticateAsync(httpContext, cancellationToken);
        if (AuthenticationHelpers.IsSameOriginOrReferer(httpContext.Request)
            && !AuthenticationHelpers.IsReferredToFromSubpath(httpContext.Request, GraphQlConstants.EndpointPath))
        {
            await ValidateAntiforgeryTokenAsync(httpContext);
        }
    }
}