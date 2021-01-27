using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionRepresentativeConnection
        : Connection<Data.Institution>
    {
        public InstitutionRepresentativeConnection(
            Data.Institution institution
        )
            : base(institution)
        {
        }

        public async Task<IEnumerable<InstitutionRepresentativeEdge>> GetEdges(
            [ScopedService] Data.ApplicationDbContext dbContext,
            CancellationToken cancellationToken
            )
        {
            return (
                await dbContext.InstitutionRepresentatives
                .Where(m => m.InstitutionId == Subject.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false)
                )
                .Select(institutionRepresentative =>
                    new InstitutionRepresentativeEdge(institutionRepresentative)
                    );
        }
    }
}