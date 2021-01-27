using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentConnection
        : Connection<Data.Institution>
    {
        public InstitutionManufacturedComponentConnection(
            Data.Institution institution
        )
            : base(institution)
        {
        }

        public async Task<IEnumerable<InstitutionManufacturedComponentEdge>> GetEdges(
            [ScopedService] Data.ApplicationDbContext dbContext,
            CancellationToken cancellationToken
            )
        {
            return (
                await dbContext.ComponentManufacturers
                .Where(m => m.InstitutionId == Subject.Id)
                .Select(m => m.ComponentId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false)
                )
                .Select(componentId =>
                    new InstitutionManufacturedComponentEdge(componentId)
                    );
        }
    }
}