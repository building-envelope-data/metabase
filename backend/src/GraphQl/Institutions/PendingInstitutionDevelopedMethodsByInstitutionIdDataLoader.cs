using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader
        : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionMethodDeveloper>
    {
        public PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.InstitutionMethodDevelopers.AsQueryable().Where(x =>
                        x.Pending && ids.Contains(x.InstitutionId)
                    ),
                x => x.InstitutionId
            )
        {
        }
    }
}