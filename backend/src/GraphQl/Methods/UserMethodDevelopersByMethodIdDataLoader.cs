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
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
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