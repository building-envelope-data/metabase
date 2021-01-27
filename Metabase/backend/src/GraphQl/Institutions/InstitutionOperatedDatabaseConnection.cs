using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionOperatedDatabaseConnection
        : Connection<Data.Institution>
    {
        public InstitutionOperatedDatabaseConnection(
            Data.Institution institution
        )
            : base(institution)
        {
        }

        public async Task<IEnumerable<InstitutionOperatedDatabaseEdge>> GetEdges(
            [ScopedService] Data.ApplicationDbContext dbContext,
            CancellationToken cancellationToken
            )
        {
            return (
                await dbContext.Databases
                    .Where(d => d.OperatorId == Subject.Id)
                    .Select(d => d.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                )
                .Select(databaseId =>
                    new InstitutionOperatedDatabaseEdge(databaseId)
                    );
        }
    }
}