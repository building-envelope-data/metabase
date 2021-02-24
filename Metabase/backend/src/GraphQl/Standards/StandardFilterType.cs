using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Standards
{
    public sealed class StandardFilterType
      : FilterInputType<Data.Standard>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.Standard> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Title);
            descriptor.Field(x => x.Abstract);
        }
    }
}