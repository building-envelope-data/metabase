using System;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.DataFormats
{
    public sealed record CreateDataFormatInput(
            string Name,
            string? Extension,
            string Description,
            string MediaType,
            Uri? SchemaLocator,
            CreateStandardInput? Standard,
            CreatePublicationInput? Publication,
            Guid ManagerId
        );
}