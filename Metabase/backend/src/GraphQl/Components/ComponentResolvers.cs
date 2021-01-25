using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl.Institutions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    public class ComponentResolvers
    {
        public async Task<IEnumerable<Data.Institution>> GetManufacturersAsync(
            Data.Component component,
            [ScopedService] Data.ApplicationDbContext dbContext,
            InstitutionByIdDataLoader institutionById,
            CancellationToken cancellationToken
            )
        {
            return (
                await institutionById.LoadAsync(
                await dbContext.Components
                .Where(c => c.Id == component.Id)
                .Include(c => c.ManufacturerEdges)
                .SelectMany(c => c.ManufacturerEdges.Select(e => e.InstitutionId))
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false),
                cancellationToken
                ).ConfigureAwait(false)
                ).Cast<Data.Institution>();
        }
    }
}