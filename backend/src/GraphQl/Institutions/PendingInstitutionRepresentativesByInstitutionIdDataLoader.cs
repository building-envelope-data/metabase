using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class PendingInstitutionRepresentativesByInstitutionIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionRepresentative>
    {
        public PendingInstitutionRepresentativesByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.InstitutionRepresentatives.AsQueryable().Where(x =>
                        x.Pending && ids.Contains(x.InstitutionId)
                    ),
                x => x.InstitutionId
                )
        {
        }
    }
}