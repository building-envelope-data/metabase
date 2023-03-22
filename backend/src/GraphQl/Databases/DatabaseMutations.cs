using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Databases
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class DatabaseMutations
    {
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateDatabasePayload> CreateDatabaseAsync(
            CreateDatabaseInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await DatabaseAuthorization.IsAuthorizedToCreateDatabaseForInstitution(
                 claimsPrincipal,
                 input.OperatorId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new CreateDatabasePayload(
                    new CreateDatabaseError(
                      CreateDatabaseErrorCode.UNAUTHORIZED,
                      "You are not authorized to create databases for the institution.",
                      new[] { nameof(input), nameof(input.OperatorId).FirstCharToLower() }
                    )
                );
            }
            if (!await context.Institutions
            .AnyAsync(
                x => x.Id == input.OperatorId,
             cancellationToken: cancellationToken
             )
            .ConfigureAwait(false)
            )
            {
                return new CreateDatabasePayload(
                    new CreateDatabaseError(
                        CreateDatabaseErrorCode.UNKNOWN_OPERATOR,
                        "Unknown operator",
                      new[] { nameof(input), nameof(input.OperatorId).FirstCharToLower() }
                    )
                );
            }
            var database = new Data.Database(
                name: input.Name,
                description: input.Description,
                locator: input.Locator
            )
            {
                OperatorId = input.OperatorId
            };
            context.Databases.Add(database);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateDatabasePayload(database);
        }

        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<UpdateDatabasePayload> UpdateDatabaseAsync(
            UpdateDatabaseInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await DatabaseAuthorization.IsAuthorizedToUpdate(
                 claimsPrincipal,
                 input.DatabaseId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new UpdateDatabasePayload(
                    new UpdateDatabaseError(
                      UpdateDatabaseErrorCode.UNAUTHORIZED,
                      "You are not authorized to update the database.",
                      new[] { nameof(input) }
                    )
                );
            }
            var database =
                await context.Databases.AsQueryable()
                .Where(i => i.Id == input.DatabaseId)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (database is null)
            {
                return new UpdateDatabasePayload(
                    new UpdateDatabaseError(
                      UpdateDatabaseErrorCode.UNKNOWN_DATABASE,
                      "Unknown database.",
                      new[] { nameof(input), nameof(input.DatabaseId).FirstCharToLower() }
                      )
                      );
            }
            database.Update(
                name: input.Name,
                description: input.Description,
                locator: input.Locator
            );
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new UpdateDatabasePayload(database);
        }
    }
}