using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.DataFormats;

[ExtendObjectType(nameof(Mutation))]
public sealed class DataFormatMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.WritePolicy)]
    public async Task<CreateDataFormatPayload> CreateDataFormatAsync(
        CreateDataFormatInput input,
        ClaimsPrincipal claimsPrincipal,
        DataFormatAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToCreateDataFormatForInstitution(
                claimsPrincipal,
                input.ManagerId,
                cancellationToken
            )
           )
        {
            return new CreateDataFormatPayload(
                new CreateDataFormatError(
                    CreateDataFormatErrorCode.UNAUTHORIZED,
                    "You are not authorized to create data formats for the institution.",
                    [nameof(input), nameof(input.ManagerId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Institutions.AsQueryable()
                .AnyAsync(
                    x => x.Id == input.ManagerId,
                    cancellationToken
                )
           )
        {
            return new CreateDataFormatPayload(
                new CreateDataFormatError(
                    CreateDataFormatErrorCode.UNKNOWN_MANAGER,
                    "Unknown manager.",
                    [nameof(input), nameof(input.ManagerId).FirstCharToLower()]
                )
            );
        }

        if (input.Reference?.Standard is not null &&
            input.Reference?.Publication is not null
           )
        {
            return new CreateDataFormatPayload(
                new CreateDataFormatError(
                    CreateDataFormatErrorCode.TWO_REFERENCES,
                    "Specify either a standard or a publication as reference.",
                    [nameof(input), nameof(input.Reference).FirstCharToLower()]
                )
            );
        }

        var dataFormat = new DataFormat(
            input.Name,
            input.Extension,
            input.Description,
            input.MediaType,
            input.SchemaLocator
        )
        {
            ManagerId = input.ManagerId,
            Reference = input.Reference?.ToDomainModel()
        };
        context.DataFormats.Add(dataFormat);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateDataFormatPayload(dataFormat);
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.WritePolicy)]
    public async Task<UpdateDataFormatPayload> UpdateDataFormatAsync(
        UpdateDataFormatInput input,
        ClaimsPrincipal claimsPrincipal,
        DataFormatAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToUpdate(
                claimsPrincipal,
                input.DataFormatId,
                cancellationToken
            )
           )
        {
            return new UpdateDataFormatPayload(
                new UpdateDataFormatError(
                    UpdateDataFormatErrorCode.UNAUTHORIZED,
                    "You are not authorized to the update data format.",
                    [nameof(input)]
                )
            );
        }

        if (input.Reference?.Standard is not null &&
            input.Reference?.Publication is not null
           )
        {
            return new UpdateDataFormatPayload(
                new UpdateDataFormatError(
                    UpdateDataFormatErrorCode.TWO_REFERENCES,
                    "Specify either a standard or a publication as reference.",
                    [nameof(input), nameof(input.Reference).FirstCharToLower()]
                )
            );
        }

        var dataFormat =
            await context.DataFormats.AsQueryable()
                .Where(i => i.Id == input.DataFormatId)
                .SingleOrDefaultAsync(cancellationToken);
        if (dataFormat is null)
        {
            return new UpdateDataFormatPayload(
                new UpdateDataFormatError(
                    UpdateDataFormatErrorCode.UNKNOWN_DATA_FORMAT,
                    "Unknown data format.",
                    [nameof(input), nameof(input.DataFormatId).FirstCharToLower()]
                )
            );
        }

        dataFormat.Update(
            input.Name,
            input.Extension,
            input.Description,
            input.MediaType,
            input.SchemaLocator
        );
        dataFormat.Reference = input.Reference?.ToDomainModel();
        await context.SaveChangesAsync(cancellationToken);
        return new UpdateDataFormatPayload(dataFormat);
    }
}