using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Extensions;

namespace Metabase.GraphQl.DataFormats
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class DataFormatMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize(Policy = Configuration.Auth.WritePolicy)]
        public async Task<CreateDataFormatPayload> CreateDataFormatAsync(
            CreateDataFormatInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
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