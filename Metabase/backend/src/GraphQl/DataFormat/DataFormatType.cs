using HotChocolate.Types;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatType
      : EntityType<Data.DataFormat, DataFormatByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.DataFormat> descriptor
            )
        {
            base.Configure(descriptor);
            descriptor
            .Field(t => t.Standard)
            .Ignore();
            descriptor
            .Field(t => t.Publication)
            .Ignore();
        }
    }
}