using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Http;
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
            ClaimsPrincipal claimsPrincipal,
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
            ClaimsPrincipal claimsPrincipal,
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

        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<VerifyDatabasePayload> VerifyDatabaseAsync(
            VerifyDatabaseInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            [Service] IHttpClientFactory httpClientFactory,
            [Service] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken
            )
        {
            if (!await DatabaseAuthorization.IsAuthorizedToVerify(
                 claimsPrincipal,
                 input.DatabaseId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new VerifyDatabasePayload(
                    new VerifyDatabaseError(
                      VerifyDatabaseErrorCode.UNAUTHORIZED,
                      "You are not authorized to verify the database.",
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
                return new VerifyDatabasePayload(
                    new VerifyDatabaseError(
                      VerifyDatabaseErrorCode.UNKNOWN_DATABASE,
                      "Unknown database.",
                      new[] { nameof(input), nameof(input.DatabaseId).FirstCharToLower() }
                      )
                      );
            }
            string queriedVerificationCode;
            try
            {
                queriedVerificationCode = await QueryVerificationCode(
                    database,
                    httpClientFactory,
                    httpContextAccessor,
                    cancellationToken
                );
            }
            catch (HttpRequestException e)
            {
                return new VerifyDatabasePayload(
                    new VerifyDatabaseError(
                      VerifyDatabaseErrorCode.REQUEST_FAILED,
                      $"Failed with status code {e.StatusCode} to request {database.Locator}.",
                      new[] { nameof(input) }
                      )
                      );
            }
            catch (JsonException e)
            {
                return new VerifyDatabasePayload(
                    new VerifyDatabaseError(
                      VerifyDatabaseErrorCode.DESERIALIZATION_FAILED,
                      $"Failed to deserialize GraphQL response of request to {database.Locator}. The details given are: Zero-based number of bytes read within the current line before the exception are {e.BytePositionInLine}, zero-based number of lines read before the exception are {e.LineNumber}, message that describes the current exception is '{e.Message}', path within the JSON where the exception was encountered is {e.Path}.",
                      new[] { nameof(input) }
                      )
                      );
            }
            if (queriedVerificationCode != database.VerificationCode)
            {
                return new VerifyDatabasePayload(
                    new VerifyDatabaseError(
                      VerifyDatabaseErrorCode.WRONG_VERIFICATION_CODE,
                      $"The verification code stored in the metabase {database.VerificationCode} does not match the one returned by the database {queriedVerificationCode}.",
                      new[] { nameof(input) }
                      )
                      );
            }
            database.Verify();
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new VerifyDatabasePayload(database);
        }

        private sealed class VerificationCodeData
        {
            public string VerificationCode { get; set; } = default!;
        }

        private static async Task<string> QueryVerificationCode(
            Data.Database database,
            [Service] IHttpClientFactory httpClientFactory,
            [Service] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken
        )
        {
            return (await QueryingDatabases.QueryDatabase<VerificationCodeData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "VerificationCode.graphql"
                        }
                    ).ConfigureAwait(false),
                    operationName: "VerificationCode"
                ),
                httpClientFactory,
                httpContextAccessor,
                cancellationToken
            ).ConfigureAwait(false)
            ).Data.VerificationCode;
        }
    }
}