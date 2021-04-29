using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users
{
    public sealed class UserRepresentedInstitutionsByUserIdDataLoader
      : Entities.AssociationsByAssociateIdDataLoader<Data.InstitutionRepresentative>
    {
        public UserRepresentedInstitutionsByUserIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.InstitutionRepresentatives.AsQueryable().Where(x =>
                        ids.Contains(x.UserId)
                    ),
                x => x.UserId
                )
        {
        }
    }
}