using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

[ExtendObjectType(nameof(Query))]
public sealed class InstitutionQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.AuthenticatedPolicy)]
    public Task<Institution?> GetCurrentInstitutionAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return authorization.SwitchUserOrApplicationAsync(
            claimsPrincipal,
            user => Task.FromResult<Institution?>(null),
            async application =>
            {
                return
                    application is null
                    ? null
                    : await context.Institutions.SingleOrDefaultAsync(_ =>
                        _.Id == application.OwnerId,
                        cancellationToken
                    );
            },
            cancellationToken
        );
    }

    [UsePaging]
    [UseFiltering<InstitutionFilterType>]
    [UseSorting<InstitutionSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Institution>> GetInstitutionsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Institutions
            .AsNoTracking()
            .Where(d => d.State == InstitutionState.VERIFIED)
            .With(resolverContext.GetQueryContext<Institution>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [UsePaging]
    [UseFiltering<InstitutionFilterType>]
    [UseSorting<InstitutionSortType>]
    [Authorize(Policy = AuthorizationPolicies.WriteScopePolicy)]
    [Authorize(Policy = AuthorizationPolicies.VerifyScopePolicy)]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Institution>> GetPendingInstitutionsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Institutions
            .AsNoTracking()
            .Where(d => d.State == InstitutionState.PENDING)
            .With(resolverContext.GetQueryContext<Institution>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<Institution?> GetInstitutionAsync(
        Guid id,
        IInstitutionByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadAsync(id, cancellationToken);
    }
}