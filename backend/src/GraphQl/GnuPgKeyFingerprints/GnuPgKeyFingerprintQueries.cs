using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
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
    public ValueTask<HotChocolate.Types.Pagination.Connection<GnuPgKeyFingerprint>> GetGnuPgKeyFingerprintsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.GnuPgKeyFingerprints
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<GnuPgKeyFingerprint>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<GnuPgKeyFingerprint?> GetGnuPgKeyFingerprintAsync(
        string fingerprint,
        IGnuPgKeyFingerprintByFingerprintDataLoader byFingerprint,
        CancellationToken cancellationToken
    )
    {
        return byFingerprint.LoadAsync(
            GnuPgKeyFingerprint.Normalize(fingerprint),
            cancellationToken
        );
    }
}