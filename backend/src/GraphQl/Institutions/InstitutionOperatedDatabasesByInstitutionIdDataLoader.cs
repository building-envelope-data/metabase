using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabasesByInstitutionIdDataLoader
    : AssociationsByAssociateIdDataLoader<Database>
{
    public InstitutionOperatedDatabasesByInstitutionIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<ApplicationDbContext> dbContextFactory
    )
        : base(
            batchScheduler,
            options,
            dbContextFactory,
            (dbContext, ids) =>
                dbContext.Databases.AsQueryable().Where(x =>
                    ids.Contains(x.OperatorId)
                ),
            x => x.OperatorId
        )
    {
    }
}