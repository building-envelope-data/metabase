using System;
using System.Linq;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedDataFormatsByInstitutionIdDataLoader
        : Entities.AssociationsByAssociateIdDataLoader<Data.DataFormat>
    {
        public InstitutionManagedDataFormatsByInstitutionIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
        )
            : base(
                batchScheduler,
                options,
                dbContextFactory,
                (dbContext, ids) =>
                    dbContext.DataFormats.AsQueryable().Where(x =>
                        ids.Contains(x.ManagerId)
                    ),
                x => x.ManagerId
            )
        {
        }
    }
}