using HotChocolate.Types;
using Metabase.GraphQl.DataX;

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
            descriptor
                .Field(t => t.Manager)
                .Type<NonNullType<ObjectType<DataFormatManagerEdge>>>()
                .Resolve(context =>
                    new DataFormatManagerEdge(
                        context.Parent<Data.DataFormat>()
                        )
                    );
            descriptor
                .Field(t => t.ManagerId)
                .Ignore();
            DataFieldConfigurator.ConfigureDataField<Data.DataFormat, DataFormatResolvers>(
                descriptor,
                "opticalData",
                _ => _.GetOpticalDataAsync(default!, default!, default!, default!, default!)
            );
            DataFieldConfigurator.ConfigureAllDataField<Data.DataFormat, DataFormatResolvers, OpticalDataPropositionInput>(
                descriptor,
                "allOpticalData",
                _ => _.GetAllOpticalDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
            DataFieldConfigurator.ConfigureHasDataField<Data.DataFormat, DataFormatResolvers, OpticalDataPropositionInput>(
                descriptor,
                "hasOpticalData",
                _ => _.GetHasOpticalDataAsync(default!, default!, default!, default!, default!)
            );
        }
    }
}