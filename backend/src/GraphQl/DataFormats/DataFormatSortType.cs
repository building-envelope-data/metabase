using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.DataFormats;

public sealed class DataFormatSortType
    : EntitySortType<DataFormat>
{
    protected override void Configure(
        ISortInputTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Extension);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.MediaType);
        descriptor.Field(x => x.SchemaLocator);
        descriptor.Field(x => x.Manager);
    }
}