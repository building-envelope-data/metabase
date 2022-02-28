using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedMethodsByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.Method>
    {
        public InstitutionManagedMethodsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.Methods.AsQueryable().Where(x =>
                        ids.Contains(x.ManagerId)
                    ),
                x => x.ManagerId
                )
        {
        }
    }
}