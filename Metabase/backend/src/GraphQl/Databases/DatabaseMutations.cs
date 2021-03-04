using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Databases
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class DatabaseMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.Auth.WritePolicy)]
        public async Task<CreateDatabasePayload> CreateDatabaseAsync(
            CreateDatabaseInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
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
                      "You are not authorized to create components for the institution.",
                      Array.Empty<string>()
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
                        "Unknown manufacturer",
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
    }
}