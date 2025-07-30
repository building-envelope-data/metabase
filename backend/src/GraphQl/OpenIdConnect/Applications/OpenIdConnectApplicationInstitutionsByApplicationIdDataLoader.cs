using System.Linq;
using GreenDonut;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationInstitutionsByApplicationIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    IDbContextFactory<ApplicationDbContext> dbContextFactory
    ) :
    AssociationsByAssociateIdDataLoader<InstitutionOpenIdConnectApplication>(
        batchScheduler,
        options,
        dbContextFactory,
        (dbContext, ids) =>
                dbContext.InstitutionOpenIdConnectApplications.AsNoTracking().Where(x =>
                    ids.Contains(x.ApplicationId)
                ),
        x => x.ApplicationId
        )
{
}