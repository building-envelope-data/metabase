using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedInstitutionsByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.Institution>
    {
        public InstitutionManagedInstitutionsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.Institutions.AsQueryable().Where(x =>
                        ids.Contains(x.ManagerId ?? Guid.Empty)
                    ),
                x => x.ManagerId ?? Guid.Empty
                )
        {
        }
    }
}