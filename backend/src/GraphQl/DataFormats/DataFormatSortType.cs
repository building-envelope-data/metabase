using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.DataFormats;

public class DataFormatSortType
    : AuditableEntitySortType<DataFormat>
{
    protected override void Configure(
        ISortInputTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Extension);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.MediaType);
        descriptor.Field(_ => _.SchemaLocator);
    }
}
