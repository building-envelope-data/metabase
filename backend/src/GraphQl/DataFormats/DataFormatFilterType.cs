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
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Extension);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.MediaType);
        descriptor.Field(_ => _.SchemaLocator);
        descriptor.Field(_ => _.Manager);
    }
}
