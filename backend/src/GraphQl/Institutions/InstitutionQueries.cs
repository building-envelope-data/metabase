using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.Enumerations;
using Guid = System.Guid;

namespace Metabase.GraphQl.Institutions;

[ExtendObjectType(nameof(Query))]
public sealed class InstitutionQueries
{
    [UsePaging]
    // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering]
    [UseSorting]
    public IQueryable<Institution> GetInstitutions(
        ApplicationDbContext context
    )
    {
        return
            context.Institutions.AsQueryable()
                .Where(d => d.State == InstitutionState.VERIFIED);
    }

    [UsePaging]
    // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering]
    [UseSorting]
    public IQueryable<Institution> GetPendingInstitutions(
        ApplicationDbContext context
    )
    {
        return
            context.Institutions.AsQueryable()
                .Where(d => d.State == InstitutionState.PENDING);
    }

    public Task<Institution?> GetInstitutionAsync(
        Guid uuid,
        InstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            uuid,
            cancellationToken
        );
    }
}