using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOwnedOpenIdConnectApplicationsByInstitutionIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    ) :
    AssociationsByAssociateIdDataLoader<OpenIdConnectApplication>(
        batchScheduler,
        options,
        dbContextFactory,
        (dbContext, ids, queryContext) =>
                dbContext.OpenIdConnectApplications.AsNoTracking().Where(x =>
                    ids.Contains(x.OwnerId)
                ).With(queryContext),
        x => x.OwnerId
        )
{
}