using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods
{
    public sealed class PendingInstitutionMethodDevelopersByMethodIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionMethodDeveloper>
    {
        public PendingInstitutionMethodDevelopersByMethodIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.InstitutionMethodDevelopers.AsQueryable().Where(x =>
                        x.Pending && ids.Contains(x.MethodId)
                    ),
                x => x.MethodId
                )
        {
        }
    }
}