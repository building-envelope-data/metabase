using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
{
    public async Task<int> GetTotalCountAsync(
        IInstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        IUserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await new InstitutionMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, InstitutionMethodDeveloper>(queryContext)
        )
            .GetTotalCountAsync(
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        +
        await new UserMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, UserMethodDeveloper>(queryContext)
        )
            .GetTotalCountAsync(
                userMethodDevelopersDataLoader,
                cancellationToken
            );
    }

    public async IAsyncEnumerable<MethodDeveloperEdge> GetEdgesAsync(
        IInstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
        IUserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var edge in new InstitutionMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, InstitutionMethodDeveloper>(queryContext)
        )
            .GetEdgesAsync(
                institutionMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
        await foreach (var edge in new UserMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, UserMethodDeveloper>(queryContext)
        )
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
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddInstitutionEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            subject.Id,
            cancellationToken
        );
    }

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddUserEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            subject.Id,
            cancellationToken
        );
    }
}

internal sealed class InstitutionMethodDeveloperConnection(
    Method subject,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : Connection<Method, InstitutionMethodDeveloper, InstitutionMethodDeveloperEdge, IInstitutionMethodDevelopersByMethodIdDataLoader>(
        subject,
        x => new InstitutionMethodDeveloperEdge(x),
        queryContext
        )
{
}

internal sealed class UserMethodDeveloperConnection(
    Method subject,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : Connection<Method, UserMethodDeveloper, UserMethodDeveloperEdge, IUserMethodDevelopersByMethodIdDataLoader>(
        subject,
        x => new UserMethodDeveloperEdge(x),
        queryContext
        )
{
}

public sealed class PendingMethodDeveloperConnection(
    Method subject,
    QueryContext<IMethodDeveloper> queryContext
    )
{
    public async Task<int> GetTotalCountAsync(
        IPendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        IPendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await new PendingInstitutionMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, InstitutionMethodDeveloper>(queryContext)
        )
            .GetTotalCountAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                cancellationToken
            )
        +
        await new PendingUserMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, UserMethodDeveloper>(queryContext)
        )
            .GetTotalCountAsync(
                pendingUserMethodDevelopersDataLoader,
                cancellationToken
            );
    }

    public async IAsyncEnumerable<MethodDeveloperEdge> GetEdgesAsync(
        IPendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
        IPendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var edge in new PendingInstitutionMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, InstitutionMethodDeveloper>(queryContext)
        )
            .GetEdgesAsync(
                pendingInstitutionMethodDevelopersDataLoader,
                cancellationToken
            )
        )
        {
            yield return new MethodDeveloperEdge(edge);
        }
        await foreach (var edge in new PendingUserMethodDeveloperConnection(
            subject,
            LiftingHelper.Lift<IMethodDeveloper, UserMethodDeveloper>(queryContext)
        )
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
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddInstitutionEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            subject.Id,
            cancellationToken
        );
    }

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddUserEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            subject.Id,
            cancellationToken
        );
    }
}

internal sealed class PendingInstitutionMethodDeveloperConnection(
    Method subject,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : AuthorizedConnection<Method, InstitutionMethodDeveloper, InstitutionMethodDeveloperEdge, IPendingInstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDeveloperAuthorization>(
        subject,
        x => new InstitutionMethodDeveloperEdge(x),
        (claimsPrincipal, method, authorization, cancellationToken) =>
            authorization.IsAuthorizedToAdd(claimsPrincipal, method.Id, cancellationToken),
        queryContext
        )
{
}

internal sealed class PendingUserMethodDeveloperConnection(
    Method subject,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : AuthorizedConnection<Method, UserMethodDeveloper, UserMethodDeveloperEdge, IPendingUserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperAuthorization>(
        subject,
        x => new UserMethodDeveloperEdge(x),
        (claimsPrincipal, method, authorization, cancellationToken) =>
            authorization.IsAuthorizedToAdd(claimsPrincipal, method.Id, cancellationToken),
        queryContext
        )
{
}

internal sealed class LiftingHelper
{
    internal static QueryContext<T> Lift<I, T>(
        QueryContext<I> queryContext
    )
        where T : I
    {
        return new QueryContext<T>(
            null, // TODO queryContext.Selector. can this be achieved with the knowledge that there are only two implementations of the interface?
            queryContext.Predicate is null ? null : Lift<I, T, bool>(queryContext.Predicate),
            queryContext.Sorting is null ? null : Lift<I, T>(queryContext.Sorting)
        );
    }

    internal static SortDefinition<T> Lift<I, T>(SortDefinition<I> source)
        where T : I
    {
        var operations = new ISortBy<T>[source.Operations.Length];
        foreach (var (index, sortBy) in source.Operations.Index())
        {
            // if (sortBy.KeySelector is Expression<Func<I, Q>> selector)
            // {
            //     operations[index] = new SortBy<T, Q>(Lift<I, T, Q>(selector));
            // }
            var qType = sortBy.KeySelector.GetType().GetGenericArguments()[0].GetGenericArguments()[1];
            var openMethod = typeof(LiftingHelper).GetMethod(
                nameof(LiftAndWrap),
                BindingFlags.NonPublic | BindingFlags.Static
            ) ?? throw new InvalidOperationException($"Could not resolve {nameof(LiftAndWrap)} method");
            var closedMethod = openMethod.MakeGenericMethod(typeof(I), typeof(T), qType);
            operations[index] = (ISortBy<T>)(closedMethod.Invoke(null, [sortBy.KeySelector]) ?? throw new InvalidOperationException("Invocation returned `null`"));
        }
        return new SortDefinition<T>(operations);
    }

    private static SortBy<T, Q> LiftAndWrap<I, T, Q>(object selector)
        where T : I
    {
        if (selector is Expression<Func<I, Q>> typedSelector)
        {
            return new SortBy<T, Q>(Lift<I, T, Q>(typedSelector));
        }
        throw new InvalidOperationException($"I don't know how to handle {selector}");
    }

    internal static Expression<Func<T, Q>> Lift<I, T, Q>(Expression<Func<I, Q>> source)
        where T : I
    {
        var newParameter = Expression.Parameter(typeof(T), source.Parameters[0].Name);
        var visitor = new ParameterReplacer(source.Parameters[0], newParameter);
        var newBody = visitor.Visit(source.Body);
        return Expression.Lambda<Func<T, Q>>(newBody, newParameter);
    }

    private sealed class ParameterReplacer(
        ParameterExpression oldParam,
        ParameterExpression newParam
    )
    : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParam ? newParam : base.VisitParameter(node);
        }
    }
}