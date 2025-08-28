using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[ExtendObjectType(nameof(Query))]
public sealed class GnuPgKeyFingerprintQueries
{
    [UsePaging]
    // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering]
    [UseSorting]
    public IQueryable<GnuPgKeyFingerprint> GetGnuPgKeyFingerprints(
        ApplicationDbContext context,
        ISortingContext sorting
    )
    {
        sorting.StabilizeOrder<GnuPgKeyFingerprint>();
        return context.GnuPgKeyFingerprints.AsNoTracking();
    }

    public Task<GnuPgKeyFingerprint?> GetGnuPgKeyFingerprintAsync(
        string fingerprint,
        GnuPgKeyFingerprintByFingerprintDataLoader byFingerprint,
        CancellationToken cancellationToken
    )
    {
        return byFingerprint.LoadAsync(fingerprint, cancellationToken);
    }
}