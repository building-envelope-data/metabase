using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentFilterType
      : FilterInputType<Data.Component>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.Component> descriptor
          )
        {
            // TODO Allow filtering by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#customization
            // This does not work right now but will in the future, see https://github.com/ChilliCream/hotchocolate/issues/2666
            descriptor.Field(x => x.Availability).Ignore();
        }
    }
}
