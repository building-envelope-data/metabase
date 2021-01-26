using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class InstitutionQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging(typeof(InstitutionType))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Institution> GetInstitutions(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Institutions;
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