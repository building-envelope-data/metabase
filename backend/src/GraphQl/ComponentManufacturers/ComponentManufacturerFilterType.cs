using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed class ComponentManufacturerFilterType
      : FilterInputType<Data.ComponentManufacturer>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.ComponentManufacturer> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Component);
            descriptor.Field(x => x.Institution);
            descriptor.Field(x => x.Pending);
        }
    }
}