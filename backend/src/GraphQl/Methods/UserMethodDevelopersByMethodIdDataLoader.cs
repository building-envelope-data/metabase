using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods
{
    public sealed class UserMethodDevelopersByMethodIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.UserMethodDeveloper>
    {
        public UserMethodDevelopersByMethodIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.UserMethodDevelopers.AsQueryable().Where(x =>
                        !x.Pending && ids.Contains(x.MethodId)
                    ),
                x => x.MethodId
                )
        {
        }
    }
}