using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
{
    private readonly Method _subject = subject;
    private readonly QueryContext<IMethodDeveloper> _queryContext = queryContext;

    public async Task<uint> GetTotalCountAsync(
        InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await new InstitutionMethodDeveloperConnection(_subject, _queryContext)
            .GetTotalCountAsync(
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        +
        await new UserMethodDeveloperConnection(_subject, _queryContext)
            .GetTotalCountAsync(
                userMethodDevelopersDataLoader,
                cancellationToken
            );
    }

    public async IAsyncEnumerable<MethodDeveloperEdge> GetEdgesAsync(
        InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var edge in new InstitutionMethodDeveloperConnection(_subject, _queryContext)
            .GetEdgesAsync(
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
        await foreach (var edge in new UserMethodDeveloperConnection(_subject, _queryContext)
            .GetEdgesAsync(
                userMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
    }

    [UseUserManager]
    public Task<bool> IsAuthorizedToAddInstitutionEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            _subject.Id,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> IsAuthorizedToAddUserEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            _subject.Id,
            cancellationToken
        );
    }
}

internal sealed class InstitutionMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
        : Connection<Method, InstitutionMethodDeveloper, InstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDeveloperEdge>(
        subject,
        x => new InstitutionMethodDeveloperEdge(x),
        null // TODO pass query context
        )
{
}

internal sealed class UserMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
        : Connection<Method, UserMethodDeveloper, UserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperEdge>(
        subject,
        x => new UserMethodDeveloperEdge(x),
        null // TODO pass query context
        )
{
}

public sealed class PendingMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
{
    private readonly Method _subject = subject;
    private readonly QueryContext<IMethodDeveloper> _queryContext = queryContext;

    public async Task<uint> GetTotalCountAsync(
        PendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        PendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await new PendingInstitutionMethodDeveloperConnection(_subject, _queryContext)
            .GetTotalCountAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                cancellationToken
            )
        +
        await new PendingUserMethodDeveloperConnection(_subject, _queryContext)
            .GetTotalCountAsync(
                pendingUserMethodDevelopersDataLoader,
                cancellationToken
            );
    }

    public async IAsyncEnumerable<MethodDeveloperEdge> GetEdgesAsync(
        PendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        PendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var edge in new PendingInstitutionMethodDeveloperConnection(_subject, _queryContext)
            .GetEdgesAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
        await foreach (var edge in new PendingUserMethodDeveloperConnection(_subject, _queryContext)
            .GetEdgesAsync(
                pendingUserMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
    }

    [UseUserManager]
    public Task<bool> IsAuthorizedToAddInstitutionEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            _subject.Id,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> IsAuthorizedToAddUserEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            _subject.Id,
            cancellationToken
        );
    }
}

internal sealed class PendingInstitutionMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
        : AuthorizedConnection<Method, InstitutionMethodDeveloper, PendingInstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDeveloperEdge, InstitutionMethodDeveloperAuthorization>(
        subject,
        x => new InstitutionMethodDeveloperEdge(x),
        (claimsPrincipal, method, authorization, cancellationToken) =>
            authorization.IsAuthorizedToAdd(claimsPrincipal, method.Id, cancellationToken),
        null // TODO pass query context
        )
{
}

internal sealed class PendingUserMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
        : AuthorizedConnection<Method, UserMethodDeveloper, PendingUserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperEdge, UserMethodDeveloperAuthorization>(
        subject,
        x => new UserMethodDeveloperEdge(x),
        (claimsPrincipal, method, authorization, cancellationToken) =>
            authorization.IsAuthorizedToAdd(claimsPrincipal, method.Id, cancellationToken),
        null // TODO pass query context
        )
{
}