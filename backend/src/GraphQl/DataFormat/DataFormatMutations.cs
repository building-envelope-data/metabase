using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.DataFormats
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class DataFormatMutations
    {
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateDataFormatPayload> CreateDataFormatAsync(
            CreateDataFormatInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await DataFormatAuthorization.IsAuthorizedToCreateDataFormatForInstitution(
                 claimsPrincipal,
                 input.ManagerId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new CreateDataFormatPayload(
                    new CreateDataFormatError(
                      CreateDataFormatErrorCode.UNAUTHORIZED,
                      "You are not authorized to create data formats for the institution.",
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
                return new CreateDataFormatPayload(
                    new CreateDataFormatError(
                        CreateDataFormatErrorCode.UNKNOWN_MANAGER,
                        "Unknown manager.",
                      new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
                    )
                );
            }
            if (input.Standard is not null &&
                input.Publication is not null
                )
            {
                return new CreateDataFormatPayload(
                    new CreateDataFormatError(
                        CreateDataFormatErrorCode.TWO_REFERENCES,
                        "Specify either a standard or a publication as reference.",
                      new[] { nameof(input), nameof(input.Publication).FirstCharToLower() }
                    )
                );
            }
            var dataFormat = new Data.DataFormat(
                name: input.Name,
                extension: input.Extension,
                description: input.Description,
                mediaType: input.MediaType,
                schemaLocator: input.SchemaLocator
            )
            {
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
            context.DataFormats.Add(dataFormat);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateDataFormatPayload(dataFormat);
        }
    }
}