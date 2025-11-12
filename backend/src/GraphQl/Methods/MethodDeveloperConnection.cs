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
    bool pending,
    QueryContext<IMethodDeveloper> queryContext
    )
{
    private readonly bool _pending = pending;
    private readonly Method _subject = subject;
    private readonly QueryContext<IMethodDeveloper> _queryContext = queryContext;

    public async Task<uint> GetTotalCountAsync(
        InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        PendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        PendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await new InstitutionMethodDeveloperConnection(_subject, _pending, _queryContext)
            .GetTotalCountAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        +
        await new UserMethodDeveloperConnection(_subject, _pending, _queryContext)
            .GetTotalCountAsync(
                pendingUserMethodDevelopersDataLoader,
                userMethodDevelopersDataLoader,
                cancellationToken
            );
    }

    public async IAsyncEnumerable<MethodDeveloperEdge> GetEdgesAsync(
        InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        PendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        PendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var edge in new InstitutionMethodDeveloperConnection(_subject, _pending, _queryContext)
            .GetEdgesAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
        await foreach (var edge in new UserMethodDeveloperConnection(_subject, _pending, _queryContext)
            .GetEdgesAsync(
                pendingUserMethodDevelopersDataLoader,
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
    bool pending,
    QueryContext<IMethodDeveloper> queryContext
    )
        : ForkingConnection<Method, InstitutionMethodDeveloper,
        PendingInstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDevelopersByMethodIdDataLoader,
        InstitutionMethodDeveloperEdge>(
        subject,
        pending,
        x => new InstitutionMethodDeveloperEdge(x),
        null // TODO pass query context
        )
{
}

internal sealed class UserMethodDeveloperConnection(
    Method subject,
    bool pending,
    QueryContext<IMethodDeveloper> queryContext
    )
        : ForkingConnection<Method, UserMethodDeveloper, PendingUserMethodDevelopersByMethodIdDataLoader,
        UserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperEdge>(
        subject,
        pending,
        x => new UserMethodDeveloperEdge(x),
        null // TODO pass query context
        )
{
}