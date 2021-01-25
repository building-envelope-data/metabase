using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl.Databases;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public class InstitutionResolvers
    {
        public async Task<IEnumerable<Data.Database>> GetOperatedDatabasesAsync(
            Data.Institution institution,
            [ScopedService] Data.ApplicationDbContext dbContext,
            DatabaseByIdDataLoader databaseById,
            CancellationToken cancellationToken
            )
        {
            return (
                await databaseById.LoadAsync(
                await dbContext.Databases
                .Where(d => d.OperatorId == institution.Id)
                .Select(d => d.Id)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false),
                cancellationToken
                ).ConfigureAwait(false)
                ).Cast<Data.Database>();
        }
    }
}