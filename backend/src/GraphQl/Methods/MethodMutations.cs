using System.Linq;
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
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.GraphQl.Methods
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class MethodMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateMethodPayload> CreateMethodAsync(
            CreateMethodInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            // TODO Authorization! In particular, which developers am I allowed to add?
            // TODO What if there are no developers (for example because they are not registered). Do we need a manager as for data formats?
            // if (!await MethodAuthorization.IsAuthorizedToCreateMethodForInstitution(
            //      claimsPrincipal,
            //      input.ManagerId,
            //      userManager,
            //      context,
            //      cancellationToken
            //      ).ConfigureAwait(false)
            // )
            // {
            //     return new CreateMethodPayload(
            //         new CreateMethodError(
            //           CreateMethodErrorCode.UNAUTHORIZED,
            //           "You are not authorized to create components for the institution.",
            //           new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
            //         )
            //     );
            // }
            var unknownInstitutionDeveloperIds =
                input.InstitutionDeveloperIds.Except(
                    await context.Users.AsQueryable()
                    .Where(x => input.InstitutionDeveloperIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownInstitutionDeveloperIds.Any())
            {
                return new CreateMethodPayload(
                    new CreateMethodError(
                      CreateMethodErrorCode.UNKNOWN_INSTITUTION_DEVELOPERS,
                      $"There are no institutions with identifier(s) {string.Join(", ", unknownInstitutionDeveloperIds)}.",
                      new[] { nameof(input), nameof(input.InstitutionDeveloperIds).FirstCharToLower() }
                      )
                );
            }
            var unknownUserDeveloperIds =
                input.UserDeveloperIds.Except(
                    await context.Users.AsQueryable()
                    .Where(u => input.UserDeveloperIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownUserDeveloperIds.Any())
            {
                return new CreateMethodPayload(
                    new CreateMethodError(
                      CreateMethodErrorCode.UNKNOWN_USER_DEVELOPERS,
                      $"There are no users with identifier(s) {string.Join(", ", unknownUserDeveloperIds)}.",
                      new[] { nameof(input), nameof(input.UserDeveloperIds).FirstCharToLower() }
                      )
                );
            }
            if (input.Standard is not null &&
                input.Publication is not null
                )
            {
                return new CreateMethodPayload(
                    new CreateMethodError(
                        CreateMethodErrorCode.TWO_REFERENCES,
                        "Specify either a standard or a publication as reference.",
                      new[] { nameof(input), nameof(input.Publication).FirstCharToLower() }
                    )
                );
            }
            var method = new Data.Method(
                name: input.Name,
                description: input.Description,
                validity: // TODO Put into helper method!
                 input.Validity is not null
                 ? new NpgsqlRange<DateTime>(
                   input.Validity.From.GetValueOrDefault(), true, input.Validity.From is null,
                   input.Validity.To.GetValueOrDefault(), true, input.Validity.To is null
                   )
                 : null,
                availability:
                 input.Availability is not null
                 ? new NpgsqlRange<DateTime>(
                   input.Availability.From.GetValueOrDefault(), true, input.Availability.From is null,
                   input.Availability.To.GetValueOrDefault(), true, input.Availability.To is null
                   )
                 : null,
                calculationLocator: input.CalculationLocator,
                categories: input.Categories
            )
            { // TODO The below is also used in `DataFormatMutations`. Put into helper!
                Standard =
                    input.Standard is null
                     ? null
                     : new Data.Standard(
                          title: input.Standard.Title,
                          @abstract: input.Standard.Abstract,
                          section: input.Standard.Section,
                          year: input.Standard.Year,
                          standardizers: input.Standard.Standardizers,
                          locator: input.Standard.Locator
                    )
                     {
                         Numeration = new Data.Numeration(
                            prefix: input.Standard.Numeration.Prefix,
                            mainNumber: input.Standard.Numeration.MainNumber,
                            suffix: input.Standard.Numeration.Suffix
                       )
                     },
                Publication =
                    input.Publication is null
                    ? null
                    : new Data.Publication(
                                title: input.Publication.Title,
                                @abstract: input.Publication.Abstract,
                                section: input.Publication.Section,
                                authors: input.Publication.Authors,
                                doi: input.Publication.Doi,
                                arXiv: input.Publication.ArXiv,
                                urn: input.Publication.Urn,
                                webAddress: input.Publication.WebAddress
                )
            };
            ;
            context.Methods.Add(method);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateMethodPayload(method);
        }
    }
}