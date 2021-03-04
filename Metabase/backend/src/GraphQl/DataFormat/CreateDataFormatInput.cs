using System;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.DataFormats
{
    public record CreateDataFormatInput(
            String Name,
            String? Extension,
            String Description,
            String MediaType,
            Uri? SchemaLocator,
            CreateStandardInput? Standard,
            CreatePublicationInput? Publication
        );
}