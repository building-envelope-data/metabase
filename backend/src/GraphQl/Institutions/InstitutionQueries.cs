using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
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
    // [UseProjection] // We disabled projections because when requesting `id` all results had the
    // same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering<InstitutionFilterType>]
    [UseSorting<InstitutionSortType>]
    public IQueryable<Institution> GetInstitutions(
        ApplicationDbContext context,
        ISortingContext sorting
    )
    {
        sorting.StabilizeOrder<Institution>();
        var institutions = context.Institutions.AsNoTracking()
                .Where(d => d.State == InstitutionState.VERIFIED);

        return institutions;
    }

    [UsePaging]
    // [UseProjection] // We disabled projections because when requesting `id` all results had the
    // same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering<InstitutionFilterType>]
    [UseSorting<InstitutionSortType>]
    [Authorize(Policy = AuthorizationPolicies.WriteScopePolicy)]
    [Authorize(Policy = AuthorizationPolicies.VerifyScopePolicy)]
    public IQueryable<Institution> GetPendingInstitutions(
        ApplicationDbContext context,
        ISortingContext sorting
    )
    {
        sorting.StabilizeOrder<Institution>();
        return
            context.Institutions.AsNoTracking()
                .Where(d => d.State == InstitutionState.PENDING);
    }

    public Task<Institution?> GetInstitutionAsync(
        Guid id,
        InstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            id,
            cancellationToken
        );
    }
}