using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class MethodMutations
    {
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateMethodPayload> CreateMethodAsync(
            CreateMethodInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            if (!await MethodAuthorization.IsAuthorizedToCreateMethodManagedByInstitution(
                    claimsPrincipal,
                    input.ManagerId,
                    userManager,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)
               )
            {
                return new CreateMethodPayload(
                    new CreateMethodError(
                        CreateMethodErrorCode.UNAUTHORIZED,
                        "You are not authorized to create methods for the institution.",
                        new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
                    )
                );
            }

            if (!await context.Institutions.AsQueryable()
                    .AnyAsync(
                        x => x.Id == input.ManagerId,
                        cancellationToken: cancellationToken
                    )
                    .ConfigureAwait(false)
               )
            {
                return new CreateMethodPayload(
                    new CreateMethodError(
                        CreateMethodErrorCode.UNKNOWN_MANAGER,
                        "Unknown manager.",
                        new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
                    )
                );
            }

            var unknownInstitutionDeveloperIds =
                input.InstitutionDeveloperIds.Except(
                    await context.Institutions.AsQueryable()
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
                validity:
                input.Validity is null
                    ? null
                    : OpenEndedDateTimeRangeType.FromInput(input.Validity),
                availability:
                input.Availability is null
                    ? null
                    : OpenEndedDateTimeRangeType.FromInput(input.Availability),
                calculationLocator: input.CalculationLocator,
                categories: input.Categories
            )
            {
                // TODO The below is also used in `DataFormatMutations`. Put into helper!
                ManagerId = input.ManagerId,
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
            foreach (var institutionDeveloperId in input.InstitutionDeveloperIds.Distinct())
            {
                method.InstitutionDeveloperEdges.Add(
                    new Data.InstitutionMethodDeveloper
                    {
                        InstitutionId = institutionDeveloperId,
                        Pending = !await InstitutionMethodDeveloperAuthorization.IsAuthorizedToConfirm(claimsPrincipal,
                            institutionDeveloperId, userManager, context, cancellationToken).ConfigureAwait(false)
                    }
                );
            }

            var loggedInUser = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            foreach (var userDeveloperId in input.UserDeveloperIds.Distinct())
            {
                method.UserDeveloperEdges.Add(
                    new Data.UserMethodDeveloper
                    {
                        UserId = userDeveloperId,
                        Pending = !await UserMethodDeveloperAuthorization
                            .IsAuthorizedToConfirm(claimsPrincipal, userDeveloperId, userManager).ConfigureAwait(false)
                    }
                );
            }

            context.Methods.Add(method);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateMethodPayload(method);
        }

        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<UpdateMethodPayload> UpdateMethodAsync(
            UpdateMethodInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            if (!await MethodAuthorization.IsAuthorizedToUpdate(
                    claimsPrincipal,
                    input.MethodId,
                    userManager,
                    context,
                    cancellationToken
                ).ConfigureAwait(false)
               )
            {
                return new UpdateMethodPayload(
                    new UpdateMethodError(
                        UpdateMethodErrorCode.UNAUTHORIZED,
                        "You are not authorized to the update method.",
                        new[] { nameof(input) }
                    )
                );
            }

            if (input.Standard is not null &&
                input.Publication is not null
               )
            {
                return new UpdateMethodPayload(
                    new UpdateMethodError(
                        UpdateMethodErrorCode.TWO_REFERENCES,
                        "Specify either a standard or a publication as reference.",
                        new[] { nameof(input), nameof(input.Publication).FirstCharToLower() }
                    )
                );
            }

            var method =
                await context.Methods.AsQueryable()
                    .Where(i => i.Id == input.MethodId)
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
            if (method is null)
            {
                return new UpdateMethodPayload(
                    new UpdateMethodError(
                        UpdateMethodErrorCode.UNKNOWN_METHOD,
                        "Unknown method.",
                        new[] { nameof(input), nameof(input.MethodId).FirstCharToLower() }
                    )
                );
            }

            method.Update(
                name: input.Name,
                description: input.Description,
                validity:
                input.Validity is null
                    ? null
                    : OpenEndedDateTimeRangeType.FromInput(input.Validity),
                availability:
                input.Availability is null
                    ? null
                    : OpenEndedDateTimeRangeType.FromInput(input.Availability),
                calculationLocator: input.CalculationLocator,
                categories: input.Categories
            );
            method.Standard =
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
                    };
            method.Publication =
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
                    );
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new UpdateMethodPayload(method);
        }
    }
}