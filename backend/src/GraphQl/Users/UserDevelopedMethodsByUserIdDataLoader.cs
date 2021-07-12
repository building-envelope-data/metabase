using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users
{
    public sealed class UserDevelopedMethodsByUserIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.UserMethodDeveloper>
    {
        public UserDevelopedMethodsByUserIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.UserMethodDevelopers.AsQueryable().Where(x =>
                        !x.Pending && ids.Contains(x.UserId)
                    ),
                x => x.UserId
                )
        {
        }
    }
}