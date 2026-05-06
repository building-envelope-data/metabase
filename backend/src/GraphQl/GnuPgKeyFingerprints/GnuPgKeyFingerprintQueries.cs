using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[ExtendObjectType(nameof(Query))]
public sealed class GnuPgKeyFingerprintQueries
{
    [UsePaging]
    [UseFiltering<GnuPgKeyFingerprintFilterType>]
    [UseSorting<GnuPgKeyFingerprintSortType>]
    [Authorize(Policy = AuthorizationPolicies.ManageGnuPgScopePolicy)]
    public ValueTask<HotChocolate.Types.Pagination.Connection<GnuPgKeyFingerprint>> GetGnuPgKeyFingerprintsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.GnuPgKeyFingerprints
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<GnuPgKeyFingerprint>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [Authorize(Policy = AuthorizationPolicies.ManageGnuPgScopePolicy)]
    public Task<GnuPgKeyFingerprint?> GetGnuPgKeyFingerprintAsync(
        string fingerprint,
        IGnuPgKeyFingerprintByFingerprintDataLoader byFingerprint,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        CancellationToken cancellationToken
    )
    {
        return byFingerprint.LoadAsync(
            GnuPgKeyFingerprint.Normalize(fingerprint),
            cancellationToken
        );
    }
}