using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.DataFormats;

public class DataFormatFilterType
    : AuditableEntityFilterType<DataFormat>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Extension);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.MediaType);
        descriptor.Field(x => x.SchemaLocator);
        descriptor.Field(x => x.Manager);
    }
}