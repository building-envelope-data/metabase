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
            // TODO Fetch `Reference` efficiently. Is it always pre-loaded automatically because it is owned or do we need a data loader?
            descriptor
                .Field(t => t.Manager)
                .Type<NonNullType<DataFormatManagerEdgeType>>()
                .Resolve(context =>
                    new DataFormatManagerEdge(
                        context.Parent<Data.DataFormat>()
                        )
                    );
            descriptor
                .Field(t => t.ManagerId).Ignore();
        }
    }
}