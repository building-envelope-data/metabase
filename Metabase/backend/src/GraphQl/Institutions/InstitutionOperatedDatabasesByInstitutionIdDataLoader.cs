using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionOperatedDatabasesByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.Database>
    {
        public InstitutionOperatedDatabasesByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.Databases.Where(x =>
                        ids.Contains(x.OperatorId)
                    ),
                x => x.OperatorId
                )
        {
        }
    }
}