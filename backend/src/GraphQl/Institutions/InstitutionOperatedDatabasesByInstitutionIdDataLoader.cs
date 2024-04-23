using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabasesByInstitutionIdDataLoader
    : Entities.AssociationsByAssociateIdDataLoader<Data.Database>
{
    public InstitutionOperatedDatabasesByInstitutionIdDataLoader(
        IBatchScheduler batchScheduler,
        DataLoaderOptions options,
        IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
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