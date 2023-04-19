using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(nameof(Query))]
    public sealed class InstitutionQueries
    {
        [UsePaging]
        // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Institution> GetInstitutions(
            Data.ApplicationDbContext context
            )
        {
            return
                context.Institutions.AsQueryable()
                .Where(d => d.State == Enumerations.InstitutionState.VERIFIED);
        }

        [UsePaging]
        // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Institution> GetPendingInstitutions(
            Data.ApplicationDbContext context
            )
        {
            return
                context.Institutions.AsQueryable()
                .Where(d => d.State == Enumerations.InstitutionState.PENDING);
        }

        public Task<Data.Institution?> GetInstitutionAsync(
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
}